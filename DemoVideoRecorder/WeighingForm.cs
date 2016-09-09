using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO.Ports;
using System.Data.SqlClient;
using Ozeki.Camera;
using _03_Onvif_Network_Video_Recorder.Properties;
using System.Globalization;
using System.Configuration;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class WeighingForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        private Font myFont, myFontBig;
        private List<string> _connectionStringList;
        private List<IpCameraHandler> _modelList;
        private List<Label> _indicatorList;
        private SerialPort _serialPort;
        private SqlConnection _dbConnection;
        private string _shipmentState = "Shp_FirstWeighing";
        private DataTable _shipmentTable;
        public WeighingForm()
        {
            InitializeComponent();
            this.Load += WeighingForm_Load;
            this.FormClosing += WeighingForm_FormClosing;

            Initialize();
        }

        void WeighingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var camera in _modelList)
            {
                if (camera.Camera != null && camera.Camera.State != CameraState.Disconnected)
                    camera.Camera.Disconnect();
            }
            DisconnectWeighingMachine();
            DisconnectDatabase();
        }

        private void Initialize()
        {
            cameraIndicator1.Parent = groupBox8;
            cameraIndicator1.Location = new Point(6, 1);
            imgCamera1.MouseDoubleClick += new MouseEventHandler(this.PicBox_DoubleClick);

            cameraIndicator2.Parent = groupBox7;
            cameraIndicator2.Location = new Point(6, 1);
            imgCamera2.MouseDoubleClick += new MouseEventHandler(this.PicBox_DoubleClick);

            cameraIndicator3.Parent = groupBox6;
            cameraIndicator3.Location = new Point(6, 1);
            imgCamera3.MouseDoubleClick += new MouseEventHandler(this.PicBox_DoubleClick);

            cameraIndicator4.Parent = groupBox5;
            cameraIndicator4.Location = new Point(6, 1);
            imgCamera4.MouseDoubleClick += new MouseEventHandler(this.PicBox_DoubleClick);

            byte[] fontData = Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8.5F);
            myFontBig = new Font(fonts.Families[0], 14F);

        }

        private void PicBox_DoubleClick(object sender, MouseEventArgs e)
        {
            CameraViewer viewer = new CameraViewer();
            viewer.PreviewImage = ((PictureBox)sender).Image;
            viewer.Show();
        }

        void WeighingForm_Load(object sender, EventArgs e)
        {
            this.Font = myFont;
            lblDiscrepency.Font = myFontBig;
            lblNetWeightLoad.Font = myFontBig;
            lblLoadedBranches.Font = myFontBig;

            foreach (ToolStripMenuItem item in ctmConfig.DropDownItems)
            {
                if (Convert.ToInt32(item.Tag) != Settings.Default.SelectedSetting)
                    item.Checked = false;
                else
                    item.Checked = true;
            }

            _connectionStringList = new List<string>();
            _modelList = new List<IpCameraHandler>();
            _indicatorList = new List<Label>();
            _shipmentTable = new DataTable();
            CreateIPCameraHandlers();
            CreateIndicators();
            CreateConnectionStrings();
            CreateSerialPort();
        }

        private void CreateSerialPort()
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = 2400;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.RtsEnable = true;
            _serialPort.Encoding = Encoding.ASCII;
            _serialPort.DataReceived +=
                new SerialDataReceivedEventHandler(_serialPort_DataReceived);

            switch (Settings.Default.SelectedSetting)
            {
                case 1:
                    _serialPort.PortName = Settings.Default.BascolPort1;
                    break;

                case 2:
                    _serialPort.PortName = Settings.Default.BascolPort2;
                    break;

                case 3:
                    _serialPort.PortName = Settings.Default.BascolPort3;
                    break;

                case 4:
                    _serialPort.PortName = Settings.Default.BascolPort4;
                    break;
            }            
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] v = new byte[8];
            int intResult = 0;
            int tryCount = 0;

            if (_serialPort.BytesToRead <= 0)
            {
            }
            else
            {
                while (_serialPort.BytesToRead > 0 && tryCount < 10)
                {

                    var output = _serialPort.Read(v, 0, 7);

                    if (output > 0)
                    {
                        try
                        {
                            intResult = Int32.Parse(System.Text.Encoding.ASCII.GetString(v, 1, 6));
                            tryCount = 10;
                        }
                        catch (FormatException)
                        {
                            tryCount++;
                        }
                    }
                    else
                        tryCount++;
                }
            }

            try
            {

                if (_shipmentState == "Shp_FirstWeighing")
                {
                    txtWeight1.Text = intResult.ToString();
                    txtDate1.Text = GetDate();
                    txtTime1.Text = GetTime();
                    txtMachine1.Text = GetMachine();

                    if (_shipmentTable.Rows.Count > 0)
                    {
                        txtWeight2.Text = "";
                        txtDate2.Text = "";
                        txtTime2.Text = "";
                        txtMachine2.Text = "";
                    }
                }
                else if (_shipmentState == "Shp_SecondWeighing")
                {
                    txtWeight2.Text = intResult.ToString();
                    txtDate2.Text = GetDate();
                    txtTime2.Text = GetTime();
                    txtMachine2.Text = GetMachine();

                    if (_shipmentTable.Rows.Count > 0)
                    {
                        txtWeight1.Text = string.Format("{0:0.###}", _shipmentTable.Rows[0].ItemArray[10]);
                        txtDate1.Text = _shipmentTable.Rows[0].ItemArray[15].ToString();
                        txtTime1.Text = _shipmentTable.Rows[0].ItemArray[17].ToString();
                        txtMachine1.Text = _shipmentTable.Rows[0].ItemArray[18].ToString();
                    }
                }
            }
            catch (Exception)
            { }
        }

        private string GetMachine()
        {
            return "7740001001";
        }

        private void CreateIndicators()
        {
            _indicatorList.Clear();
            _indicatorList.Add(cameraIndicator1);
            _indicatorList.Add(cameraIndicator2);
            _indicatorList.Add(cameraIndicator3);
            _indicatorList.Add(cameraIndicator4);
        }
        private void InvokeGuiThread(Action action)
        {
            if (IsHandleCreated)
            {
                BeginInvoke(action);
            }
        }
        private void CreateIPCameraHandlers()
        {
            _modelList.Clear();
            var i = 0;
            while (i < 4)
            {
                _modelList.Add(new IpCameraHandler());
                i++;
            }
            foreach (var item in _modelList)
            {
                item.CameraStateChanged += ModelCameraStateChanged;
                //item.CameraErrorOccurred += ModelCameraErrorOccurred;
            }
        }

        private void ModelCameraErrorOccurred(object sender, Ozeki.Camera.CameraErrorEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                IpCameraHandler cc = sender as IpCameraHandler;
                var cameraAddress = cc.Camera.CameraAddress.Split(':');
            });
        }

        private void ModelCameraStateChanged(object sender, Ozeki.Camera.CameraStateEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                IpCameraHandler cc = sender as IpCameraHandler;
                if (cc.Camera != null)
                {
                    var cameraAddress = cc.Camera.CameraAddress.Split(':');
                    switch (e.State)
                    {
                        case CameraState.Connected:
                            if (_connectionStringList[0].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[0].Text = "فعال";
                                _indicatorList[0].ForeColor = Color.Green;
                            }
                            else if (_connectionStringList[1].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[1].Text = "فعال";
                                _indicatorList[1].ForeColor = Color.Green;
                            }
                            else if (_connectionStringList[2].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[2].Text = "فعال";
                                _indicatorList[2].ForeColor = Color.Green;
                            }
                            else if (_connectionStringList[3].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[3].Text = "فعال";
                                _indicatorList[3].ForeColor = Color.Green;
                            }
                            break;

                        case CameraState.Disconnected:
                            if (_connectionStringList[0].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[0].Text = "غیرفعال";
                                _indicatorList[0].ForeColor = Color.Red;
                                //_modelList[0].Camera.Dispose();
                            }
                            else if (_connectionStringList[1].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[1].Text = "غیرفعال";
                                _indicatorList[1].ForeColor = Color.Red;
                                //_modelList[1].Camera.Dispose();
                            }
                            else if (_connectionStringList[2].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[2].Text = "غیرفعال";
                                _indicatorList[2].ForeColor = Color.Red;
                                //_modelList[2].Camera.Dispose();
                            }
                            else if (_connectionStringList[3].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[3].Text = "غیرفعال";
                                _indicatorList[3].ForeColor = Color.Red;
                                //_modelList[3].Camera.Dispose();
                            }
                            break;

                        case CameraState.Connecting:
                            if (_connectionStringList[0].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[0].Text = "در حال اتصال";
                                _indicatorList[0].ForeColor = Color.Orange;
                            }
                            else if (_connectionStringList[1].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[1].Text = "در حال اتصال";
                                _indicatorList[1].ForeColor = Color.Orange;
                            }
                            else if (_connectionStringList[2].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[2].Text = "در حال اتصال";
                                _indicatorList[2].ForeColor = Color.Orange;
                            }
                            else if (_connectionStringList[3].StartsWith(cameraAddress[0]))
                            {
                                _indicatorList[3].Text = "در حال اتصال";
                                _indicatorList[3].ForeColor = Color.Orange;
                            }
                            break;
                    }
                }
            });
        }

        private string GetTime()
        {
            return string.Format("{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        private string GetDate()
        {
            string GregorianDate = DateTime.Now.ToString();
            DateTime d = DateTime.Parse(GregorianDate);
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
        }

        private void CreateConnectionStrings()
        {
            _connectionStringList.Clear();
            switch (Settings.Default.SelectedSetting)
            {
                case 1:
                    _connectionStringList.Add(Settings.Default.CameraIP11 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP12 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP13 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP14 + ":80;Username=root;Password=49091;Transport=TCP;");
                    break;

                case 2:
                    _connectionStringList.Add(Settings.Default.CameraIP21 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP22 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP23 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP24 + ":80;Username=root;Password=49091;Transport=TCP;");
                    break;

                case 3:
                    _connectionStringList.Add(Settings.Default.CameraIP31 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP32 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP33 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP34 + ":80;Username=root;Password=49091;Transport=TCP;");
                    break;

                case 4:
                    _connectionStringList.Add(Settings.Default.CameraIP41 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP42 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP43 + ":80;Username=root;Password=49091;Transport=TCP;");
                    _connectionStringList.Add(Settings.Default.CameraIP44 + ":80;Username=root;Password=49091;Transport=TCP;");
                    break;
            }
        }

        private void ConnectIpCam()
        {
            try
            {
                var i = 0;
                while (i < _modelList.Count)
                {
                    if (_modelList[i] == null && _modelList[i].Camera.State == CameraState.Connected) return;
                    _modelList[i].ConnectOnvifCamera(_connectionStringList[i]);
                    i++;
                }
            }
            catch(Exception)
            { }
        }

        private void ConnectDatabase()
        {
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["AshaDbContext"].ConnectionString;
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(connection);
            if (_dbConnection.State != ConnectionState.Open)
            {
                try
                {
                    _dbConnection.Open();
                    InvokeGuiThread(() =>
                        {
                            DatabaseIndicator.Text = "فعال";
                            DatabaseIndicator.ForeColor = Color.Green;
                        });
                }
                catch (Exception)
                {
                    InvokeGuiThread(() =>
                        {
                            DatabaseIndicator.Text = "غیرفعال";
                            DatabaseIndicator.ForeColor = Color.Red;
                        });
                }
            }
        }

        private void ConnectWeighingMachine()
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                    WeighingMachineIndicator.Text = "فعال";
                    WeighingMachineIndicator.ForeColor = Color.Green;
                }
                catch (Exception)
                {
                    WeighingMachineIndicator.Text = "غیرفعال";
                    WeighingMachineIndicator.ForeColor = Color.Red;
                }
            }
        }

        private void DisconnectDatabase()
        {
            if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
            {
                _dbConnection.Close();
                DatabaseIndicator.Text = "غیرفعال";
                DatabaseIndicator.ForeColor = Color.Red;
            }
        }

        private void DisconnectWeighingMachine()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                WeighingMachineIndicator.Text = "غیرفعال";
                WeighingMachineIndicator.ForeColor = Color.Red;
            }
        }

        private void DisconnectIpCam()
        {
            try
            {
                for (int i = 0; i < _modelList.Count; i++)
                {
                    if ((_modelList[i] == null || _modelList[i].Camera == null) || _modelList[i].Camera.State == CameraState.Disconnected) continue;
                    {
                        _modelList[i].Disconnect();
                        //_modelList[i].Camera.Disconnect();
                    }
                }
            }
            catch (Exception)
            { }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            string path = "C:\\";
            for (int i = 0; i < 4; i++)
            {
                if (_modelList[i] == null || _modelList[i].Camera == null) continue;
                
                var date = _modelList[i].Camera.Host.ToString() + "-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                var image = _modelList[i].CreateSnapShot(path);

                if (image != null)
                {
                    switch (i)
                    {
                        case 0:
                            imgCamera1.Image = new Bitmap(image);
                            imgCamera1.Image.Tag = date;
                            break;
                        case 1:
                            imgCamera2.Image = new Bitmap(image);
                            imgCamera2.Image.Tag = date;
                            break;
                        case 2:
                            imgCamera3.Image = new Bitmap(image);
                            imgCamera3.Image.Tag = date;
                            break;
                        case 3:
                            imgCamera4.Image = new Bitmap(image);
                            imgCamera4.Image.Tag = date;
                            break;
                    }

                }
            }

            if (_shipmentState == "Shp_SecondWeighing")
            {
                double weight1, weight2;
                if (double.TryParse(txtWeight1.Text, out weight1) && double.TryParse(txtWeight2.Text, out weight2))
                {
                    lblNetWeightLoad.Text = string.Format("{0:0.###}", Math.Abs(weight2 - weight1));

                    double netWeight, estimatedWeight;
                    if (double.TryParse(lblNetWeightLoad.Text, out netWeight) && double.TryParse(_shipmentTable.Rows[0].ItemArray[21].ToString(), out estimatedWeight))
                    {
                        lblDiscrepency.Text = string.Format("{0:0.###}", Math.Abs((estimatedWeight-netWeight)/estimatedWeight*100));
                    }
                }
                
            }
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (_shipmentTable.Rows.Count <= 0)
            {
                MessageBox.Show("این محموله در وضعیت توزین قرار ندارد", "خطا", MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Image[] images = { imgCamera1.Image, imgCamera2.Image, imgCamera3.Image, imgCamera4.Image };
            SqlCommand sqlCommand;

            try
            {
                if (_shipmentState == "Shp_FirstWeighing" &&
                    MessageBox.Show("اطلاعات به بارگیری ارسال خواهد شد. آیا مطمئن هستید؟", "تکمیل توزین", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    == System.Windows.Forms.DialogResult.OK)
                {
                    sqlCommand = new SqlCommand("UPDATE SDSO_Shipment SET TruckWeight=@TruckWeight, StartTime=@StartTime " +
                                "WHERE Code = @ShipmentCode", _dbConnection);
                    sqlCommand.Parameters.AddWithValue("@ShipmentCode", _shipmentTable.Rows[0].ItemArray[20].ToString());
                    sqlCommand.Parameters.AddWithValue("@TruckWeight", txtWeight1.Text);
                    sqlCommand.Parameters.AddWithValue("@StartTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand("SDSO_001_ShipmentStatus", _dbConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    sqlCommand.Parameters.Add("@ShipmentCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@StatusCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@NewStatusCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    sqlCommand.Parameters["@shipmentCode"].Value = _shipmentTable.Rows[0].ItemArray[20].ToString();
                    sqlCommand.Parameters["@StatusCode"].Value = "Shp_FirstWeighing";
                    sqlCommand.Parameters["@NewStatusCode"].Value = "Shp_Loading";
                    sqlCommand.Parameters["@PositionCode"].Value = "Pos_999";
                    sqlCommand.Parameters["@CreatorCode"].Value = "1";
                    sqlCommand.Parameters["@ReturnMessage"].Value = "";
                    sqlCommand.Parameters["@ReturnValue"].Value = 1;

                    sqlCommand.ExecuteNonQuery();
                    string returnMessage = Convert.ToString(sqlCommand.Parameters["@ReturnMessage"].Value);
                    MessageBox.Show(returnMessage, "پیغام", MessageBoxButtons.OK,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    sqlCommand.Dispose();

                    foreach (var item in images)
                    {
                        if (item == null) continue;
                        var image = imageToByteArray(item);
                        var date = item.Tag;

                        if (image != null)
                        {
                            sqlCommand = new SqlCommand("INSERT INTO SIDev_Binary (BinaryTitle, BinaryPath, BinaryData, BinaryExt, BinarySize, CreatorID, AttachDate, Embedded, Guid)" +
                                                                       "VALUES (@date, @date, @Image, '.jpg', @ImageSize, 1, GETDATE(), 1, NEWID())", _dbConnection);
                            sqlCommand.Parameters.AddWithValue("@date", date);
                            sqlCommand.Parameters.AddWithValue("@Image", image);
                            sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();

                            sqlCommand = new SqlCommand("SELECT ID, Guid FROM SIDev_Binary WHERE BinaryTitle = '" + date + "'", _dbConnection);
                            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                            DataTable BinaryTable = new DataTable();
                            sqlAdapter.Fill(BinaryTable);
                            sqlCommand.Dispose();

                            sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                                                                "VALUES (2631, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnection);
                            sqlCommand.Parameters.AddWithValue("@MainGuid", _shipmentTable.Rows[0].ItemArray[6].ToString());
                            sqlCommand.Parameters.AddWithValue("@RelatedGuid", BinaryTable.Rows[0].ItemArray[1].ToString());
                            sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Dispose();
                        }
                    }

                    ClearFields();
                }
                else if (_shipmentState == "Shp_SecondWeighing" &&
                    MessageBox.Show("اطلاعات به دیسپچینگ ارسال خواهد شد. آیا مطمئن هستید؟", "تکمیل توزین", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    == System.Windows.Forms.DialogResult.OK)
                {
                    sqlCommand = new SqlCommand("UPDATE SDSO_Shipment SET LoadedTruckWeight=@LoadedTruckWeight, NetWeight=@NetWeight, EndTime=@EndTime " +
                                "WHERE Code = @ShipmentCode", _dbConnection);
                    sqlCommand.Parameters.AddWithValue("@ShipmentCode", _shipmentTable.Rows[0].ItemArray[20].ToString());
                    sqlCommand.Parameters.AddWithValue("@NetWeight", lblNetWeightLoad.Text);
                    sqlCommand.Parameters.AddWithValue("@LoadedTruckWeight", txtWeight2.Text);
                    sqlCommand.Parameters.AddWithValue("@EndTime", DateTime.Now);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand("SDSO_001_ShipmentStatus", _dbConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    sqlCommand.Parameters.Add("@ShipmentCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@StatusCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@NewStatusCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    sqlCommand.Parameters["@shipmentCode"].Value = _shipmentTable.Rows[0].ItemArray[20].ToString();
                    sqlCommand.Parameters["@StatusCode"].Value = "Shp_SecondWeighing";
                    sqlCommand.Parameters["@NewStatusCode"].Value = "Shp_Issue";
                    sqlCommand.Parameters["@PositionCode"].Value = "Pos_999";
                    sqlCommand.Parameters["@CreatorCode"].Value = "1";
                    sqlCommand.Parameters["@ReturnMessage"].Value = "";
                    sqlCommand.Parameters["@ReturnValue"].Value = 1;

                    sqlCommand.ExecuteNonQuery();
                    string returnMessage = Convert.ToString(sqlCommand.Parameters["@ReturnMessage"].Value);
                    int returnValue = Convert.ToInt32(sqlCommand.Parameters["@ReturnValue"].Value);

                    

                    if (returnValue == 1)
                    {
                        MessageBox.Show(returnMessage, "پیغام", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        sqlCommand.Dispose();

                        foreach (var item in images)
                        {
                            if (item == null) continue;
                            var image = imageToByteArray(item);
                            var date = item.Tag;

                            if (image != null)
                            {
                                sqlCommand = new SqlCommand("INSERT INTO SIDev_Binary (BinaryTitle, BinaryPath, BinaryData, BinaryExt, BinarySize, CreatorID, AttachDate, Embedded, Guid)" +
                                                                           "VALUES (@date, @date, @Image, '.jpg', @ImageSize, 1, GETDATE(), 1, NEWID())", _dbConnection);
                                sqlCommand.Parameters.AddWithValue("@date", date);
                                sqlCommand.Parameters.AddWithValue("@Image", image);
                                sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                                sqlCommand.ExecuteNonQuery();
                                sqlCommand.Dispose();

                                sqlCommand = new SqlCommand("SELECT ID, Guid FROM SIDev_Binary WHERE BinaryTitle = '" + date + "'", _dbConnection);
                                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                                DataTable BinaryTable = new DataTable();
                                sqlAdapter.Fill(BinaryTable);
                                sqlCommand.Dispose();

                                sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                                                                    "VALUES (2631, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnection);
                                sqlCommand.Parameters.AddWithValue("@MainGuid", _shipmentTable.Rows[0].ItemArray[6].ToString());
                                sqlCommand.Parameters.AddWithValue("@RelatedGuid", BinaryTable.Rows[0].ItemArray[1].ToString());
                                sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                                sqlCommand.ExecuteNonQuery();
                                sqlCommand.Dispose();
                            }
                        }
                        ClearFields();
                    }
                    else if (returnValue == 0)
                    {
                        if (MessageBox.Show(returnMessage, "اخطار", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                        {
                            sqlCommand.Dispose();

                            if (MessageBox.Show("آیا مغایرت وزنی تایید می شود؟", "پیغام", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                            {
                                sqlCommand = new SqlCommand("SDSO_001_ShipmentWeightApprove", _dbConnection);
                                sqlCommand.CommandType = CommandType.StoredProcedure;

                                // set up the parameters
                                sqlCommand.Parameters.Add("@ShipmentCode", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                                sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                                // set parameter values
                                sqlCommand.Parameters["@shipmentCode"].Value = _shipmentTable.Rows[0].ItemArray[20].ToString();
                                sqlCommand.Parameters["@PositionCode"].Value = "Pos_999";
                                sqlCommand.Parameters["@CreatorCode"].Value = "1";
                                sqlCommand.Parameters["@ReturnMessage"].Value = "";
                                sqlCommand.Parameters["@ReturnValue"].Value = 1;

                                sqlCommand.ExecuteNonQuery();
                                sqlCommand.Dispose();

                                foreach (var item in images)
                                {
                                    if (item == null) continue;
                                    var image = imageToByteArray(item);
                                    var date = item.Tag;

                                    if (image != null)
                                    {
                                        sqlCommand = new SqlCommand("INSERT INTO SIDev_Binary (BinaryTitle, BinaryPath, BinaryData, BinaryExt, BinarySize, CreatorID, AttachDate, Embedded, Guid)" +
                                                                                   "VALUES (@date, @date, @Image, '.jpg', @ImageSize, 1, GETDATE(), 1, NEWID())", _dbConnection);
                                        sqlCommand.Parameters.AddWithValue("@date", date);
                                        sqlCommand.Parameters.AddWithValue("@Image", image);
                                        sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                                        sqlCommand.ExecuteNonQuery();
                                        sqlCommand.Dispose();

                                        sqlCommand = new SqlCommand("SELECT ID, Guid FROM SIDev_Binary WHERE BinaryTitle = '" + date + "'", _dbConnection);
                                        SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                                        DataTable BinaryTable = new DataTable();
                                        sqlAdapter.Fill(BinaryTable);
                                        sqlCommand.Dispose();

                                        sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                                                                            "VALUES (2631, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnection);
                                        sqlCommand.Parameters.AddWithValue("@MainGuid", _shipmentTable.Rows[0].ItemArray[6].ToString());
                                        sqlCommand.Parameters.AddWithValue("@RelatedGuid", BinaryTable.Rows[0].ItemArray[1].ToString());
                                        sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                                        sqlCommand.ExecuteNonQuery();
                                        sqlCommand.Dispose();
                                    }
                                }
                                ClearFields();
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                InvokeGuiThread(() =>
                {
                    DatabaseIndicator.Text = "غیرفعال";
                    DatabaseIndicator.ForeColor = Color.Red;
                });
                MessageBox.Show("لطفا به پایگاد داده متصل شوید", "Database Connection Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(SqlException exp)
            {
                MessageBox.Show(exp.Message, "SQL Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            lblBillOfLading.Text = "---";
            lblCar.Text = "---";
            lblDestination.Text = "---";
            lblDriver.Text = "---";
            lblDriverLicence.Text = "---";
            lblSaler.Text = "---";
            lblSender.Text = "---";
            lblSource.Text = "---";
            txtDate1.Text = "";
            txtDate2.Text = "";
            txtMachine1.Text = "";
            txtMachine2.Text = "";
            txtShipmentCode.Text = "";
            txtTime1.Text = "";
            txtTime2.Text = "";
            txtWeight1.Text = "";
            txtWeight2.Text = "";
            lblLoadedBranches.Text = "0";
            lblNetWeightLoad.Text = "0";
            lblDiscrepency.Text = "0";
            _shipmentTable.Clear();
            dgShipmentDetail.DataSource = null;
            _shipmentState = "Shp_FirstWeighing";
            imgCamera1.Image = null;
            imgCamera2.Image = null;
            imgCamera3.Image = null;
            imgCamera4.Image = null;
        }

        private void btnShipmentSearch_Click(object sender, EventArgs e)
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();

            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT        SDSO_Shipment.Title AS ShipmentTitle, SDSO_Shipment.TransportCode, SDSO_Customer.Title AS Destination, WMLog_Vehicle.CarrierNumber, " +
                                                           "                     WMLog_Driver.Title AS DriverTitle, WMLog_Driver.LicenseNumber, SDSO_Shipment.Guid, SDSO_Shipment.FormStatusCode AS ShipmentStatus, " +
                                                           "                     contact1.Title AS TransportCompany, SISys_Location.Title AS City, SDSO_Shipment.TruckWeight, SDSO_Shipment.LoadedTruckWeight, " +
                                                           "                     SDSO_Shipment.NetWeight, SDSO_Shipment.EstimatedWeight, LEFT(dbo.MiladiToShamsi(SDSO_Shipment.EndTime), 10) AS EndDate, LEFT(dbo.MiladiToShamsi(SDSO_Shipment.StartTime), 10) AS StartDate, " +
                                                           "                     RIGHT(dbo.MiladiToShamsi(SDSO_Shipment.EndTime), 8) AS EndTime, RIGHT(dbo.MiladiToShamsi(SDSO_Shipment.StartTime), 8) AS StartTime, " +
                                                           "                     FirstWeighingMachineCode, SecondWeighingMachineCode, SDSO_Shipment.Code, SDSO_Shipment.EstimatedWeight, sdso_customer.CustomerCode " +
                                                           " FROM		  SDSO_Shipment LEFT OUTER JOIN SDSO_Customer " +
		                                                   "         ON	SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode LEFT OUTER JOIN WMLog_Vehicle " +
		                                                   "         ON	SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN WMLog_Driver " +
		                                                   "         ON	SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WFFC_Contact AS contact1 " +
		                                                   "         ON	SDSO_Shipment.TransportCompanyCode = contact1.Code LEFT OUTER JOIN WFFC_Contact AS contact2 " +
		                                                   "        ON	SDSO_Customer.CustomerCode = contact2.code LEFT OUTER JOIN SISys_Location " +
                                                           "         ON	contact2.GeograghyLocationCode = SISys_Location.Code " +
                                                           " WHERE        (SDSO_Shipment.FormStatusCode LIKE '%Weighing%') AND SDSO_Shipment.Code LIKE '%" + txtShipmentCode.Text + "%'"
                                                            , _dbConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        _shipmentTable.Clear();
                        da.Fill(_shipmentTable);
                        if (_shipmentTable.Rows.Count > 0)
                        {
                            lblSender.Text = _shipmentTable.Rows[0].ItemArray[8].ToString();
                            lblSaler.Text = _shipmentTable.Rows[0].ItemArray[2].ToString();
                            lblBillOfLading.Text = _shipmentTable.Rows[0].ItemArray[1].ToString();
                            lblDestination.Text = _shipmentTable.Rows[0].ItemArray[9].ToString();
                            lblCar.Text = _shipmentTable.Rows[0].ItemArray[3].ToString();
                            lblDriver.Text = _shipmentTable.Rows[0].ItemArray[4].ToString();
                            lblDriverLicence.Text = _shipmentTable.Rows[0].ItemArray[5].ToString();
                            txtWeight1.Text = string.Format("{0:0.###}", _shipmentTable.Rows[0].ItemArray[10]);
                            txtWeight2.Text = string.Format("{0:0.###}", _shipmentTable.Rows[0].ItemArray[11]);
                            txtDate1.Text = _shipmentTable.Rows[0].ItemArray[15].ToString();
                            txtDate2.Text = _shipmentTable.Rows[0].ItemArray[14].ToString();
                            txtTime1.Text = _shipmentTable.Rows[0].ItemArray[17].ToString();
                            txtTime2.Text = _shipmentTable.Rows[0].ItemArray[16].ToString();
                            txtMachine1.Text = _shipmentTable.Rows[0].ItemArray[18].ToString();
                            txtMachine2.Text = _shipmentTable.Rows[0].ItemArray[19].ToString();
                            _shipmentState = _shipmentTable.Rows[0].ItemArray[7].ToString();
                        }
                        else
                        {
                            lblDriver.Text = "---";
                            lblBillOfLading.Text = "---";
                            lblDestination.Text = "---";
                            lblCar.Text = "---";
                            lblDriverLicence.Text = "---";
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT  Sequence as ردیف, PartSerialCode as [بارکد شمش], ProductCode as [کد کالا], WMInv_Part.Title as [نام کالا], ShipmentAuthorizeCode as [مجوز حمل] FROM SDSO_Shipment " +
                        "INNER JOIN SDSO_ShipmentDetail ON SDSO_Shipment.Code = SDSO_ShipmentDetail.ShipmentCode " +
                        "INNER JOIN WMInv_Part ON SDSO_ShipmentDetail.ProductCode = WMInv_Part.Code WHERE SDSO_Shipment.FormStatusCode Like '%Weighing%' AND SDSO_Shipment.Code Like '%" + txtShipmentCode.Text + "%'"
                                                            , _dbConnection))
                    {
                        DataTable shipmentDetailTable = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(shipmentDetailTable);
                        if (shipmentDetailTable.Rows.Count > 0)
                        {
                            dgShipmentDetail.DataSource = shipmentDetailTable;

                            var results = shipmentDetailTable.AsEnumerable().Count();
                            lblLoadedBranches.Text = string.Format("{0:0.###}", results);
                        }
                    }
                }
            }
            catch (Exception)
            {
                InvokeGuiThread(() =>
                {
                    DatabaseIndicator.Text = "غیرفعال";
                    DatabaseIndicator.ForeColor = Color.Red;
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread thread0 = new System.Threading.Thread(ConnectIpCam);
            System.Threading.Thread thread1 = new System.Threading.Thread(ConnectDatabase);
            System.Threading.Thread thread2 = new System.Threading.Thread(ConnectWeighingMachine);
            thread0.Start();
            thread1.Start();
            thread2.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisconnectIpCam();
            DisconnectWeighingMachine();
            DisconnectDatabase();
        }

        private void txtShipmentCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnShipmentSearch_Click(sender, e);
            }
        }

        private void بزرگنماییToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraViewer viewer = new CameraViewer();
            viewer.PreviewImage = ((PictureBox)((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl).Image;
            viewer.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShipmentListForm shipments = new ShipmentListForm();
            if(shipments.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtShipmentCode.Text = shipments.shipmentCode;
            }
        }

        private void contextMenu_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                menuItem.Checked = true;
                Settings.Default.SelectedSetting = Convert.ToInt32(menuItem.Tag);
                Settings.Default.Save();

                var parentMenu = ((ToolStripDropDownMenu)(((ToolStripMenuItem)sender).Owner));
                foreach (ToolStripMenuItem item in parentMenu.Items)
                {
                    if (item != menuItem)
                    {
                        item.Checked = false;
                    }
                }
            }

            DisconnectIpCam();
            DisconnectWeighingMachine();
            CreateConnectionStrings();
            CreateSerialPort();

            System.Threading.Thread thread0 = new System.Threading.Thread(ConnectIpCam);
            System.Threading.Thread thread2 = new System.Threading.Thread(ConnectWeighingMachine);
            thread0.Start();
            thread2.Start();
        }
    }
}
