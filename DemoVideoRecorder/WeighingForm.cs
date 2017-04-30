using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Net;
using System.IO;
using AshaWeighing.Properties;

namespace AshaWeighing
{
    public partial class WeighingForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        private bool _isStable = false;
        private Font myFont, myFontBig;
        private List<Label> _indicatorList;
        private SerialPort _serialPort;
        private SqlConnection _dbConnection;
        private string _weighingOrderCode = string.Empty;
        private DataTable _WeighingDetail = new DataTable();
        private string _weighingDetailCriteria = string.Empty;
        private string _weighingMasterCriteria = string.Empty;
        private string _weighingStatusCriteria = string.Empty;
        private DataTable _WeighingOrderTable;
        private DataTable _weighingOrderDetailTable;
        private bool _negativeWeight = false;
        private double _emptyWeight = -1;
        private double _estimatedWeight = -1;
        private double _secondQuantity = -1;
        private int _indicatorWeight = 0;
        const int bufsize = 32 * 1024;

        private Dictionary<string, string> _configs;
        private List<WeighingOrderType> _weighingTypes;
        private System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
        public bool IsStable
        {
            get { return _isStable; }
            set
            {
                _isStable = value;
                if(_isStable)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        requestFrame(i);
                    }
                    sevenSegmentWeight.Value = _indicatorWeight.ToString();
                    btnGetStableData.Text = "فعال سازی دریافت اطلاعات";
                    sevenSegmentWeight.ColorLight = Color.LightGreen;
                    btnSaveData.Enabled = true;
                }
                else
                {
                    imgCamera1.Image = null;
                    imgCamera2.Image = null;
                    imgCamera3.Image = null;
                    imgCamera4.Image = null;

                    sevenSegmentWeight.Value = _indicatorWeight.ToString();
                    btnGetStableData.Text = "تثبیت وزن و دریافت تصاویر";
                    sevenSegmentWeight.ColorLight = Color.Red;
                    btnSaveData.Enabled = false;
                }
            }
        }
        public WeighingForm()
        {
            InitializeComponent();
            Load += WeighingForm_Load;
            FormClosing += WeighingForm_FormClosing;
        }

        private void requestFrame(int requestNumber)
        {
            int counter = requestNumber;
            InvokeGuiThread(() =>
            {
                _indicatorList[requestNumber].Text = "در حال اتصال";
                _indicatorList[requestNumber].ForeColor = Color.Orange;
            });

            if (!string.IsNullOrEmpty(Globals.CameraAddress[requestNumber]))
            {
                string cameraUrl = Globals.CameraAddress[requestNumber];
                try
                {
                    HttpWebRequest request = WebRequest.Create(cameraUrl) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.Method = WebRequestMethods.Http.Get;
                    request.Credentials = new NetworkCredential(Globals.CameraUsername[requestNumber], Globals.CameraPassword[requestNumber]);
                    request.Proxy = null;
                    request.BeginGetResponse(new AsyncCallback(finishRequestFrame), request);
                }
                catch (UriFormatException exp)
                {
                    LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "requestFrame: " + exp.Message, Globals.UserCode);
                    MessageBox.Show("تنظیمات دوربین " + (requestNumber + 1) + " صحیح نمیباشد. لطفا با مدیر سیستم تماس بگیرید");
                }
            }
            else
            {
                InvokeGuiThread(() =>
                {
                    _indicatorList[requestNumber].Text = "عدم اتصال";
                    _indicatorList[requestNumber].ForeColor = Color.Blue;
                });
            }
        }

        void finishRequestFrame(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                MemoryStream memoryStream = new MemoryStream();

                using (Stream responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(memoryStream);

                    using (Bitmap frame = new Bitmap(memoryStream))
                    {
                        if (frame != null)
                        {
                            if (response.ResponseUri.OriginalString == Globals.CameraAddress[0])
                            {
                                imgCamera1.Image = (Bitmap)frame.Clone();
                                imgCamera1.Image.Tag = "Camera1-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                                    DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                                InvokeGuiThread(() =>
                                {
                                    _indicatorList[0].Text = "فعال";
                                    _indicatorList[0].ForeColor = Color.Green;
                                });
                            }
                            else if (response.ResponseUri.OriginalString == Globals.CameraAddress[1])
                            {
                                imgCamera2.Image = (Bitmap)frame.Clone();
                                imgCamera2.Image.Tag = "Camera2-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                                    DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                                InvokeGuiThread(() =>
                                {
                                    _indicatorList[1].Text = "فعال";
                                    _indicatorList[1].ForeColor = Color.Green;
                                });
                            }
                            else if (response.ResponseUri.OriginalString == Globals.CameraAddress[2])
                            {
                                imgCamera3.Image = (Bitmap)frame.Clone();
                                imgCamera3.Image.Tag = "Camera3-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                                    DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                                InvokeGuiThread(() =>
                                {
                                    _indicatorList[2].Text = "فعال";
                                    _indicatorList[2].ForeColor = Color.Green;
                                });
                            }
                            else if (response.ResponseUri.OriginalString == Globals.CameraAddress[3])
                            {
                                imgCamera4.Image = (Bitmap)frame.Clone();
                                imgCamera4.Image.Tag = "Camera4-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                                    DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                                InvokeGuiThread(() =>
                                {
                                    _indicatorList[3].Text = "فعال";
                                    _indicatorList[3].ForeColor = Color.Green;
                                });
                            }
                        }
                    }
                }
            }
            catch(Exception exp)
            {
                LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "finishRequestFrame: " + exp.Message, Globals.UserCode);
                for (int i = 0; i < 4; i++)
                {
                    int counter = i;
                    if (_indicatorList[counter].ForeColor == Color.Orange)
                    {
                        InvokeGuiThread(() =>
                        {
                            _indicatorList[counter].Text = "غیر فعال";
                            _indicatorList[counter].ForeColor = Color.Red;
                        });
                    }
                }
            }
        }
        void WeighingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectWeighingMachine();
            DisconnectDatabase();
        }

        private void InitializeConfigurations()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT SISys_SubSysConfig.Code, SISys_SubSysConfig.Title FROM SISys_SubSysConfig JOIN SISys_SubSysConfigDetail " +
                                                       "ON SISys_SubSysConfig.Code = SISys_SubSysConfigDetail.ConfigurationCode " +
                                                       "WHERE SubSystemCode='WMLog' AND FormStatusCode='Cfg_Active' and Value='Visible' "
                                                        , _dbConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable conf = new DataTable();
                    da.Fill(conf);

                    if (conf.Rows.Count > 0)
                    {
                        _configs = new Dictionary<string, string>();
                        ToolStripMenuItem[] items = new ToolStripMenuItem[conf.Rows.Count];

                        for (int i = 0; i < conf.Rows.Count; i++)
                        {
                            _configs.Add(conf.Rows[i].Field<string>("Code"), conf.Rows[i].Field<string>("Title"));

                            items[i] = new ToolStripMenuItem();
                            items[i].Name = "dynamicItem" + i.ToString();
                            items[i].Tag = conf.Rows[i].Field<string>("Code");
                            items[i].Text = conf.Rows[i].Field<string>("Title");
                            items[i].Click += new EventHandler(contextMenu_Click);
                        }

                        ctmConfig.DropDownItems.AddRange(items);
                    }
                    else
                    {
                        if (MessageBox.Show("تنظیمات کامپیوتر شما در سیستم ثبت نشده است. آیا می‌خواهید تنظیمات پیش فرض برای شما ثبت شود؟", "راه اندازی تنظیمات",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            var macAddr =
                            (
                                from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                                where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                                select nic.GetPhysicalAddress().ToString()
                            ).FirstOrDefault();

                            string input = Microsoft.VisualBasic.Interaction.InputBox("تنظیمات به نام چه کامپیوتری ثبت شود؟", "ثبت تنظیمات", "تنظیمات در کامپیوتر " + macAddr);
                            using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO SISys_SubSysConfigDetail(ConfigurationCode, Code, Title, Value, CreatorCode, CreationDate) " +
                                                    "VALUES (@ConfigurationCode, @Code, @Title, @Value, @CreatorCode, @CreationDate)", _dbConnection))
                            {

                                // set up the parameters
                                sqlCommand.Parameters.Add("@ConfigurationCode", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@Code", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                                sqlCommand.Parameters.Add("@CreationDate", SqlDbType.DateTime);


                                using (SqlCommand cmd2 = new SqlCommand("SELECT Code, Title FROM SISys_SubSysConfig WHERE SubSystemCode='WMLog' AND FormStatusCode='Cfg_Active'"
                                                                , _dbConnection))
                                {
                                    da = new SqlDataAdapter(cmd2);
                                    conf = new DataTable();
                                    da.Fill(conf);
                                    ToolStripMenuItem[] items = new ToolStripMenuItem[conf.Rows.Count];

                                    for (int i = 0; i < conf.Rows.Count; i++)
                                    {
                                        // set parameter values
                                        sqlCommand.Parameters["@ConfigurationCode"].Value = conf.Rows[i].Field<string>("Code");
                                        sqlCommand.Parameters["@Code"].Value = macAddr;
                                        sqlCommand.Parameters["@Title"].Value = input;
                                        sqlCommand.Parameters["@Value"].Value = "Visible";
                                        sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                                        sqlCommand.Parameters["@CreationDate"].Value = DateTime.Now;

                                        sqlCommand.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("لطفاً جهت ثبت تنظیمات با مدیر سیستم تماس بگیرید.", "راه اندازی تنظیمات", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            Globals.GetConfigurationDetails(Settings.Default.SelectedConfiguration);
        }

        private void InitializeFontAndCamera()
        {
            cameraIndicator1.Parent = groupBox8;
            cameraIndicator1.Location = new Point(6, 1);
            imgCamera1.MouseDoubleClick += new MouseEventHandler(PicBox_DoubleClick);

            cameraIndicator2.Parent = groupBox7;
            cameraIndicator2.Location = new Point(6, 1);
            imgCamera2.MouseDoubleClick += new MouseEventHandler(PicBox_DoubleClick);

            cameraIndicator3.Parent = groupBox6;
            cameraIndicator3.Location = new Point(6, 1);
            imgCamera3.MouseDoubleClick += new MouseEventHandler(PicBox_DoubleClick);

            cameraIndicator4.Parent = groupBox5;
            cameraIndicator4.Location = new Point(6, 1);
            imgCamera4.MouseDoubleClick += new MouseEventHandler(PicBox_DoubleClick);

            byte[] fontData = AshaWeighing.Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8.5F);
            myFontBig = new Font(fonts.Families[0], 14F);

            Font = myFont;
            lblDiscrepency.Font = myFontBig;
            lblNetWeight.Font = myFontBig;
            lblLoadedBranches.Font = myFontBig;
            lblCurrentTime.Font = myFontBig;
            lblCurrentDate.Font = myFontBig;
            lblWeighingBridgeCode.Font = myFontBig;
            weighingBridgeIndicator.Font = myFontBig;
            lblWeighingResponsible.Font = myFontBig;
        }

        private void PicBox_DoubleClick(object sender, MouseEventArgs e)
        {
            CameraViewer viewer = new CameraViewer();
            viewer.PreviewImage = ((PictureBox)sender).Image;
            viewer.Show();
        }

        void WeighingForm_Load(object sender, EventArgs e)
        {
            InitializeFontAndCamera();

            _indicatorList = new List<Label>();
            _WeighingOrderTable = new DataTable();
            CreateIndicators();
            ConnectDatabase();
            InitializeConfigurations();
            CreateSerialPort();
            ConnectWeighingMachine();
            Thread thread3 = new Thread(ConnectCameras);
            thread3.Start();
            GetWeighingTypes();
            foreach (ToolStripMenuItem item in ctmConfig.DropDownItems)
            {
                if (((string)item.Tag) != Settings.Default.SelectedConfiguration)
                    item.Checked = false;
                else
                {
                    item.Checked = true;
                    lblWeighingBridgeCode.Text = item.Text;
                }
            }

            lblWeighingResponsible.Text = Globals.UserName;
            tmr.Interval = 1000;//ticks every 1 second
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Start();
        }

        private void GetWeighingTypes()
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();
            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM WMLog_WeighingType"
                                                            , _dbConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable wieghingTypeTable = new DataTable();
                        da.Fill(wieghingTypeTable);
                        _weighingTypes = new List<WeighingOrderType>();
                        for (int i = 0; i < wieghingTypeTable.Rows.Count; i++)
                        {
                            _weighingTypes.Add(new WeighingOrderType() { Name = wieghingTypeTable.Rows[i].Field<string>("Title"),
                                                                        Value = wieghingTypeTable.Rows[i].Field<string>("Code") });
                        }

                        cmbWeighingTypes.DataSource = _weighingTypes;
                        cmbWeighingTypes.DisplayMember = "Name";
                        cmbWeighingTypes.ValueMember = "Value";
                        cmbWeighingTypes.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.Database, _weighingOrderCode, "Error", "GetWeighingTypes: " + ex.Message, Globals.UserCode);
                InvokeGuiThread(() =>
                {
                    DatabaseIndicator.Text = "غیرفعال";
                    DatabaseIndicator.ForeColor = Color.Red;
                });
            }
        }

        //change the label text inside the tick event
        private void tmr_Tick(object sender, EventArgs e)
        {
            lblCurrentDate.Text = GetDate();
            lblCurrentTime.Text = GetTime();
        }

        private void CreateSerialPort()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = Globals.WeighingMachineSerialPort;  
            _serialPort.BaudRate = Globals.SerialPorBaudRate;
            _serialPort.DataBits = Globals.SerialPortDataBits;
            _serialPort.Parity = Globals.SerialPortParity;
            _serialPort.Handshake = Globals.SerialPortHandshake;
            _serialPort.StopBits = Globals.SerialPortStopBits;
            _serialPort.RtsEnable = true;
            _serialPort.Encoding = Encoding.ASCII;
            _serialPort.DataReceived +=
                new SerialDataReceivedEventHandler(_serialPort_DataReceived);   
        }

        public void ShowNegativeWeightMessageBox()
        {
            LogHelper.Log(LogTarget.Database, "Warning", _weighingOrderCode, "ShowNegativeWeightMessageBox: Negative weight detected.", Globals.UserCode);
            var thread = new Thread(
            () =>
            {
              if(MessageBox.Show("وزن باسکول منفی می باشد! لطفا دستگاه را بررسی نمایید") == DialogResult.OK)
              {
                    _negativeWeight = false;
                    btnSaveData.Enabled = true;
                    btnGetStableData.Enabled = true;
              }
            });
          thread.Start();
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

                    if (output == 7)
                    {
                        try
                        {
                            StringBuilder hex = new StringBuilder(2);
                            hex.AppendFormat("{0:x2}", v[0]);

                            if (hex.ToString().ToLower().Equals("bb"))
                            {
                                hex.Clear();
                                hex.AppendFormat("{0:x2}", v[1]);

                                if (hex.ToString().ToLower().Equals("e0"))
                                {
                                    intResult = -10 * int.Parse(Encoding.ASCII.GetString(v, 2, 6));
                                    tryCount = 10;
                                }
                                else
                                {
                                    intResult = int.Parse(Encoding.ASCII.GetString(v, 1, 6));
                                    tryCount = 10;
                                }
                            }


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
                if (intResult < -10)
                {
                    _indicatorWeight = intResult;
                    sevenSegmentWeight.Value = intResult.ToString();

                    if (sevenSegmentWeight.ColorBackground != Color.Red || sevenSegmentWeight.ColorLight != Color.Yellow)
                    {
                        sevenSegmentWeight.ColorBackground = Color.Red;
                        sevenSegmentWeight.ColorLight = Color.Yellow;
                    }

                    if (_negativeWeight == false)
                    {
                        _negativeWeight = true;
                        btnSaveData.Enabled = false;
                        btnGetStableData.Enabled = false;
                        ShowNegativeWeightMessageBox();
                    }

                }
                else if (intResult >= 0)
                {
                    _indicatorWeight = intResult;

                    if (IsStable && Math.Abs(int.Parse(sevenSegmentWeight.Value) - _indicatorWeight) == 0)
                    {
                        sevenSegmentWeight.ColorBackground = Color.Black;
                        sevenSegmentWeight.ColorLight = Color.Green;
                    }
                    else if (IsStable && Globals.Tolerance > 0 && Math.Abs(int.Parse(sevenSegmentWeight.Value) - _indicatorWeight) <= Globals.Tolerance)
                    {
                        sevenSegmentWeight.ColorBackground = Color.Black;
                        sevenSegmentWeight.ColorLight = Color.Yellow;
                    }
                    else
                    {
                        sevenSegmentWeight.ColorBackground = Color.Black;
                        sevenSegmentWeight.ColorLight = Color.Red;
                        IsStable = false;
                        sevenSegmentWeight.Value = _indicatorWeight.ToString();
                    }
                }

                

                if (_weighingOrderDetailTable.Rows.Count > 0)
                {
                    double weight1;
                    if (double.TryParse(sevenSegmentWeight.Value, out weight1) && _emptyWeight > 0)
                    {
                        lblNetWeight.Text = string.Format("{0:0.###}", Math.Abs(_emptyWeight - weight1));

                        double netWeight;
                        if (double.TryParse(lblNetWeight.Text, out netWeight) && _estimatedWeight > 0)
                        {
                            lblDiscrepency.Text = string.Format("{0:0.###}", (_estimatedWeight - netWeight) / _estimatedWeight * 100);
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }

        private string GetMachine()
        {
            return Globals.WeighingMachineCode;
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

        private string GetTime()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        private string GetDate()
        {
            string GregorianDate = DateTime.Now.ToString();
            DateTime d = DateTime.Parse(GregorianDate);
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
        }
        private void ConnectCameras()
        {
            for (int i = 0; i < 4; i++)
            {
                var counter = i;

                InvokeGuiThread(() =>
                {
                    _indicatorList[counter].Text = "در حال اتصال";
                    _indicatorList[counter].ForeColor = Color.Orange;
                });

                if (!string.IsNullOrEmpty(Globals.CameraAddress[i]))
                {
                    string cameraUrl = Globals.CameraAddress[i];
                    HttpWebResponse response = null;
                    var request = (HttpWebRequest)WebRequest.Create(cameraUrl);
                    request.Credentials = new NetworkCredential(Globals.CameraUsername[i], Globals.CameraPassword[i]);
                    request.Proxy = null;
                    request.Method = "HEAD";

                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        InvokeGuiThread(() =>
                        {
                            _indicatorList[counter].Text = "فعال";
                            _indicatorList[counter].ForeColor = Color.Green;
                        });
                    }
                    catch (WebException ex)
                    {
                        if (_indicatorList[counter].ForeColor == Color.Orange)
                        {
                            LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "ConnectCamera: " + ex.Message, Globals.UserCode);
                            /* A WebException will be thrown if the status of the response is not `200 OK` */
                            InvokeGuiThread(() =>
                            {
                                _indicatorList[counter].Text = "غیر فعال";
                                _indicatorList[counter].ForeColor = Color.Red;
                            });
                        }
                    }
                    finally
                    {
                        // Don't forget to close your response.
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                }
            }
        }
        private void ConnectDatabase()
        {
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(Globals.ConnectionString);
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
                catch (Exception exp)
                {
                    LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "ConnectDatabase: " + exp.Message, Globals.UserCode);
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
            if (_serialPort != null && !_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                    InvokeGuiThread(() =>
                    {
                        weighingBridgeIndicator.Text = "فعال";
                        weighingBridgeIndicator.ForeColor = Color.Green;
                    });
                }
                catch (Exception exp)
                {
                    LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "ConnectWeighingMachine: " + exp.Message, Globals.UserCode);
                    InvokeGuiThread(() =>
                    {
                        weighingBridgeIndicator.Text = "غیرفعال";
                        weighingBridgeIndicator.ForeColor = Color.Red;
                    });
                }
            }
            else
            {
                InvokeGuiThread(() =>
                {
                    weighingBridgeIndicator.Text = "غیرفعال";
                    weighingBridgeIndicator.ForeColor = Color.Red;
                });
            }
        }

        private void DisconnectDatabase()
        {
            if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
            {
                _dbConnection.Close();
            }
        }

        private void DisconnectWeighingMachine()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                sevenSegmentWeight.Value = "0";
                weighingBridgeIndicator.Text = "غیرفعال";
                weighingBridgeIndicator.ForeColor = Color.Red;
            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnGetStableData_Click(object sender, EventArgs e)
        {
            IsStable = !IsStable;
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnGetStableData_Click: Pressed.", Globals.UserCode);
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnSaveData_Click: Pressed.", Globals.UserCode);
            if (_weighingOrderCode == null)
            {
                MessageBox.Show("شناسه توزین مشخص نشده است. لطفا شناسه توزین را جستجو نمایید.", "خطا", MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Image[] images = { imgCamera1.Image, imgCamera2.Image, imgCamera3.Image, imgCamera4.Image };
            SqlCommand sqlCommand;

            try
            {
                if (MessageBox.Show("اطلاعات دخیره خواهد شد. آیا مطمئن هستید؟", "تکمیل توزین", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    == DialogResult.OK)
                {
                    sqlCommand = new SqlCommand("WMLog_000_InsertWeighing", _dbConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    sqlCommand.Parameters.Add("@WeighingOrderCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@WeighingTypeCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@Weight", SqlDbType.Int);
                    sqlCommand.Parameters.Add("@Image1", SqlDbType.Binary);
                    sqlCommand.Parameters.Add("@Image2", SqlDbType.Binary);
                    sqlCommand.Parameters.Add("@Image3", SqlDbType.Binary);
                    sqlCommand.Parameters.Add("@Image4", SqlDbType.Binary);
                    sqlCommand.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ResponsibleCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    sqlCommand.Parameters["@WeighingOrderCode"].Value = _weighingOrderCode;
                    sqlCommand.Parameters["@WeighingTypeCode"].Value = cmbWeighingTypes.SelectedValue;
                    sqlCommand.Parameters["@Weight"].Value = int.Parse(sevenSegmentWeight.Value);
                    sqlCommand.Parameters["@Image1"].Value = imageToByteArray(imgCamera1.Image);
                    sqlCommand.Parameters["@Image2"].Value = imageToByteArray(imgCamera2.Image);
                    sqlCommand.Parameters["@Image3"].Value = imageToByteArray(imgCamera3.Image);
                    sqlCommand.Parameters["@Image4"].Value = imageToByteArray(imgCamera4.Image);
                    sqlCommand.Parameters["@MachineCode"].Value = Globals.WeighingMachineCode;
                    sqlCommand.Parameters["@ResponsibleCode"].Value = Globals.PersonnelCode;
                    sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                    sqlCommand.Parameters["@ReturnMessage"].Value = "";
                    sqlCommand.Parameters["@ReturnValue"].Value = 1;

                    sqlCommand.ExecuteNonQuery();
                    string returnMessage = Convert.ToString(sqlCommand.Parameters["@ReturnMessage"].Value);
                    int returnValue = Convert.ToInt32(sqlCommand.Parameters["@ReturnValue"].Value);

                    LogHelper.Log(LogTarget.Database, "Database", _weighingOrderCode, "btnSaveData_Click: Weighing data saved. " +
                                "WeighingOrderCode=" + _weighingOrderCode +
                                ", CreatorCode=" + Globals.UserName + 
                                ", SavedWeight=" + sevenSegmentWeight.Value +
                                ", IndicatorWeight=" + _indicatorWeight, Globals.UserCode);

                    if (returnValue == 1)
                    {
                        MessageBox.Show(returnMessage, "پیغام", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        sqlCommand.Dispose();
                    }
                    else if (returnValue == 0)
                    {
                        MessageBox.Show(returnMessage, "اخطار", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        sqlCommand.Dispose();
                    }
                    ClearFields();
                }
            }
            catch (InvalidOperationException ex)
            {
                LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "btnSaveData_Click: " + ex.Message, Globals.UserCode);
                InvokeGuiThread(() =>
                {
                    DatabaseIndicator.Text = "غیرفعال";
                    DatabaseIndicator.ForeColor = Color.Red;
                });
                MessageBox.Show("لطفا به پایگاد داده متصل شوید", "Database Connection Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(SqlException exp)
            {
                LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "btnSaveData_Click: " + exp.Message, Globals.UserCode);
                MessageBox.Show(exp.Message, "SQL Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            _weighingOrderCode = null;

            txtWeighingOrderCode.Text = "";
            //sevenSegmentWeight.Value = "0";
            IsStable = false;
            btnGetStableData.Enabled = true;
            btnSaveData.Enabled = false;
            lblLoadedBranches.Text = "0";
            lblNetWeight.Text = "0";
            lblDiscrepency.Text = "0";
            _WeighingOrderTable.Clear();
            dgWeighingOrderDetail.DataSource = null;
            dgWeighingData.DataSource = null;
            dgShipmentDetail.DataSource = null;
            imgCamera1.Image = null;
            imgCamera2.Image = null;
            imgCamera3.Image = null;
            imgCamera4.Image = null;
            _emptyWeight = -1;
            _estimatedWeight = -1;
        }

        private void btnWeighingOrderSearch_Click(object sender, EventArgs e)
        {
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnWeighingOrderSearch_Click: Pressed.", Globals.UserCode);
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();

            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    if (!string.IsNullOrEmpty(txtWeighingOrderCode.Text))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT Code, Title, WeighingTypeCode FROM WMLog_WeighingOrder where "
                                        + "FormStatusCode='Wgh_Weighing' and "
                                        + "(POShipmentCode='" + txtWeighingOrderCode.Text
                                        + "' OR SOShipmentCode='" + txtWeighingOrderCode.Text + "' "
                                        + "OR InvTransactionCode='" + txtWeighingOrderCode.Text
                                        + "' OR Reference='" + txtWeighingOrderCode.Text
                                        + "' OR Code='" + txtWeighingOrderCode.Text + "')"
                                        , _dbConnection))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            _WeighingOrderTable.Clear();
                            da.Fill(_WeighingOrderTable);
                            if (_WeighingOrderTable.Rows.Count > 1)
                            {
                                DataRow[] results = _WeighingOrderTable.Select("WeighingTypeCode='" + cmbWeighingTypes.SelectedValue + "'");
                                if (results.Count() > 1)
                                    MessageBox.Show("بیش از یک شناسه توزین باز برای این کد وجود دارد. لطفاً با مدیر سیستم تماس بگیرید.", "خطا در سیستم توزین");
                                else
                                    _weighingOrderCode = results[0].Field<string>("Code");
                            }
                            if (_WeighingOrderTable.Rows.Count == 1)
                            {
                                _weighingOrderCode = _WeighingOrderTable.Rows[0].Field<string>("Code");
                                cmbWeighingTypes.SelectedValue = _WeighingOrderTable.Rows[0].Field<string>("WeighingTypeCode");
                                cmbWeighingTypes_SelectedValueChanged(cmbWeighingTypes, new EventArgs());
                            }
                            else
                            {
                                MessageBox.Show("شناسه توزین باز با کد مذکور در سیستم وجود ندارد. لطفاً با مدیر سیستم تماس بگیرید.", "خطا در سیستم توزین");
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(_weighingMasterCriteria.Replace("@Code", _weighingOrderCode)
                                        , _dbConnection))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable weighingFieldsTable = new DataTable();
                            weighingFieldsTable.Clear();
                            da.Fill(weighingFieldsTable);
                            if (weighingFieldsTable.Rows.Count > 0)
                            {
                                dgWeighingData.DataSource = weighingFieldsTable;
                                dgWeighingData.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                dgWeighingData.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            }
                            else
                            {
                                MessageBox.Show("دریافت اطلاعات تکمیلی توزین با خطا مواجه شد. لطفا با مدیر سیستم تماس بگیرید.", "خطا در سیستم توزین");
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(_weighingDetailCriteria.Replace("@Code", _weighingOrderCode)
                                        , _dbConnection))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            _WeighingDetail.Clear();
                            da.Fill(_WeighingDetail);
                            if (_WeighingDetail.Rows.Count > 0)
                            {
                                dgShipmentDetail.DataSource = _WeighingDetail;
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand("SELECT WMLog_WeighingOrderDetail.OperationSequence AS [ردیف], WMLog_WeighingOperation.Title AS [توزین], "
                                        + "SISys_FormStatus.Title AS[وضعیت], CONVERT(Decimal(10, 0), WMLog_WeighingOrderDetail.Weight) AS[وزن], dbo.MiladiTOShamsi(WeighingDateTime) AS[تاریخ توزین], "
                                        + "HREA_Personnel.Title AS[توزینکار], MRMA_Machine.Title AS[باسکول] "
                                        + "FROM WMLog_WeighingOrderDetail LEFT OUTER JOIN WMLog_WeighingOperation "
                                        + "ON WMLog_WeighingOrderDetail.OperationCode = WMLog_WeighingOperation.Code LEFT OUTER JOIN SISys_FormStatus "
                                        + "ON WMLog_WeighingOrderDetail.OperationStatusCode = SISys_FormStatus.Code LEFT OUTER JOIN MRMA_Machine "
                                        + "ON WMLog_WeighingOrderDetail.MachineCode = MRMA_Machine.Code LEFT OUTER JOIN HREA_Personnel "
                                        + "ON WMLog_WeighingOrderDetail.ResponsibleCode = HREA_Personnel.PersonnelCode "
                                        + "WHERE WeighingOrderCode='" + _weighingOrderCode + "'"
                                                                , _dbConnection))
                        {
                            _weighingOrderDetailTable = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(_weighingOrderDetailTable);
                            if (_weighingOrderDetailTable.Rows.Count > 0)
                            {
                                dgWeighingOrderDetail.DataSource = _weighingOrderDetailTable;

                                var results = _weighingOrderDetailTable.AsEnumerable().Count();
                            }
                            else
                            {
                                dgWeighingOrderDetail.DataSource = null;
                            }
                        }
                        using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(WMLog_WeighingOrderDetail.Weight, 0) AS [Weight], "
                                        + "ISNULL(WMLog_WeighingOrderDetail.EstimatedQuantity, 0) AS [EstimatedQuantity], "
                                        + "ISNULL(WMLog_WeighingOrderDetail.SecondQuantity, 0) AS [SecondQuantity] "
                                        + "FROM WMLog_WeighingOrderDetail "
                                        + "WHERE WeighingOrderCode='" + _weighingOrderCode + "' AND OperationStatusCode='LogOpr_Completed' ORDER BY OperationSequence DESC"
                                                                , _dbConnection))
                        {
                            DataTable WeightTable = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(WeightTable);
                            if (WeightTable.Rows.Count > 0)
                            {
                                _emptyWeight = decimal.ToDouble(WeightTable.Rows[0].Field<decimal>("Weight"));
                                _estimatedWeight = decimal.ToDouble(WeightTable.Rows[0].Field<decimal>("EstimatedQuantity"));
                                _secondQuantity = decimal.ToDouble(WeightTable.Rows[0].Field<decimal>("SecondQuantity"));
                                lblLoadedBranches.Text = _secondQuantity.ToString();
                            }
                            else
                            {
                                _emptyWeight = -1;
                                _estimatedWeight = -1;
                                _secondQuantity = -1;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                var t = exp;
            }
            
            try
            {
                using (SqlCommand cmd = new SqlCommand(_weighingStatusCriteria
                                    , _dbConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable weighingStatusTable = new DataTable();
                    weighingStatusTable.Clear();
                    da.Fill(weighingStatusTable);
                    if (weighingStatusTable.Rows.Count > 0)
                    {
                        dgWeighingStatus.DataSource = weighingStatusTable;
                        dgWeighingStatus.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgWeighingStatus.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
            }
            catch (Exception exp)
            {
                var j = exp;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnConnect_Click: Pressed.", Globals.UserCode);
            Thread thread1 = new Thread(ConnectDatabase);
            Thread thread2 = new Thread(ConnectWeighingMachine);
            Thread thread3 = new Thread(ConnectCameras);
            thread1.Start();
            thread2.Start();
            thread3.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisconnectWeighingMachine();
            DisconnectDatabase();
        }

        private void txtShipmentCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnWeighingOrderSearch_Click(sender, e);
            }
        }

        private void بزرگنماییToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraViewer viewer = new CameraViewer();
            viewer.PreviewImage = ((PictureBox)((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl).Image;
            viewer.Show();
        }

        private void btnOpenWeighingList_Click(object sender, EventArgs e)
        {
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnOpenWeighingList_Click: Pressed.", Globals.UserCode);
            ShipmentListForm shipments = new ShipmentListForm();
            shipments.OrderType = (string)cmbWeighingTypes.SelectedValue;
            if (shipments.ShowDialog() == DialogResult.OK)
            {
                cmbWeighingTypes.SelectedValue = shipments.OrderType;
                txtWeighingOrderCode.Text = shipments.shipmentCode;
            }
        }

        private void contextMenu_Click(object sender, EventArgs e)
        {
            App.toolStripStatusLabel1.Text = "بررسی اتصال دستگاه ها...";
            Cursor.Current = Cursors.WaitCursor;
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                menuItem.Checked = true;
                Settings.Default.SelectedConfiguration = (string)menuItem.Tag;
                Settings.Default.Save();

                var parentMenu = ((ToolStripDropDownMenu)(((ToolStripMenuItem)sender).Owner));
                foreach (ToolStripMenuItem item in parentMenu.Items)
                {
                    if (item != menuItem)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        lblWeighingBridgeCode.Text = item.Text;
                    }
                }
            }

            ClearFields();
            Globals.GetConfigurationDetails(Settings.Default.SelectedConfiguration);
            DisconnectWeighingMachine();
            CreateSerialPort();
            Thread thread3 = new Thread(ConnectCameras);
            thread3.Start();

            Thread thread2 = new Thread(ConnectWeighingMachine);
            thread2.Start();
            Cursor.Current = Cursors.Default;
            App.toolStripStatusLabel1.Text = "";
        }

        private void cmbWeighingTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();

            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT MasterCriteria, DetailCriteria, StatusCriteria "
                            + "FROM WMLog_WeighingType WHERE Code='" + cmbWeighingTypes.SelectedValue + "'"
                                                            , _dbConnection))
                    {
                        DataTable weighingTypeDetailTable = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(weighingTypeDetailTable);
                        if (weighingTypeDetailTable.Rows.Count > 0)
                        {
                            _weighingDetailCriteria = weighingTypeDetailTable.Rows[0].Field<string>("DetailCriteria");
                            _weighingMasterCriteria = weighingTypeDetailTable.Rows[0].Field<string>("MasterCriteria");
                            _weighingStatusCriteria = weighingTypeDetailTable.Rows[0].Field<string>("StatusCriteria");
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception exp)
            {
                LogHelper.Log(LogTarget.Database, "Error", _weighingOrderCode, "cmbWeighingTypes_SelectedValueChanged: " + exp.Message, Globals.UserCode);
                InvokeGuiThread(() =>
                {
                    DatabaseIndicator.Text = "غیرفعال";
                    DatabaseIndicator.ForeColor = Color.Red;
                });
            }
        }

        private void dgWeighingData_SelectionChanged(object sender, EventArgs e)
        {
            dgWeighingData.ClearSelection();
        }

        private void dgWeighingStatus_SelectionChanged(object sender, EventArgs e)
        {
            dgWeighingStatus.ClearSelection();
        }

        private void dgWeighingOrderDetail_SelectionChanged(object sender, EventArgs e)
        {
            dgWeighingOrderDetail.ClearSelection();
        }

        private void txtWeighingOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnWeighingOrderSearch_Click(this, new EventArgs());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LogHelper.Log(LogTarget.Database, "Event", _weighingOrderCode, "btnClear_Click: Pressed.", Globals.UserCode);
            ClearFields();
        }
    }
}
