using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Ozeki.Media;
using _03_Onvif_Network_Video_Recorder.LOG;
using Ozeki.Camera;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Text;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class MainForm : Form
    {
        private SqlConnection _dbConnector;
        private SqlCommand _dbCommand;
        private SqlDataAdapter _dbAdapter;
        private DataTable _shipmentTable;

        private VideoViewerWF _currentVideoViewer;

        private List<string> _connectionStringList;

        private List<VideoViewerWF> _videoViewerList;

        private List<IpCameraHandler> ModelList;

        private CameraURLBuilderWF _myCameraUrlBuilder;

        public MainForm()
        {
            InitializeComponent();

            Log.OnLogMessageReceived += Log_OnLogMessageReceived;

            _myCameraUrlBuilder = new CameraURLBuilderWF();

            _connectionStringList = new List<string>();
            _videoViewerList = new List<VideoViewerWF>();
            ModelList = new List<IpCameraHandler>();

            
            CreateVideoViewers();
            CreateIPCameraHandlers();
            //_dbConnector = new SqlConnection("Data Source=tcp:172.20.1.30;initial catalog=AshaMES_PASCO_V03;persist security info=True;user id=sa;password=@sh@3rp;MultipleActiveResultSets=True;" );
            //_dbConnector.Open();
            
            //Asha.Repo.AshaDbContext ctx = new Asha.Repo.AshaDbContext();
        }

        private void CreateConnectionStrings()
        {
            _connectionStringList.Clear();
            if (radioButton_60Ton.Checked && radioButton_Input.Checked)
            {
                _connectionStringList.Add("172.20.200.1:80;Username=root;Password=;Transport=TCP;");
                _connectionStringList.Add("172.20.200.2:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.9:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.10:80;Username=root;Password=49091;Transport=TCP;");
            }
            else if (radioButton_80Ton.Checked && radioButton_Input.Checked)
            {
                _connectionStringList.Add("172.20.200.3:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.4:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.11:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.12:80;Username=root;Password=49091;Transport=TCP;");
            }
            else if (radioButton_60Ton.Checked && radioButton_Output.Checked)
            {
                _connectionStringList.Add("172.20.200.5:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.6:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.13:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.14:80;Username=root;Password=49091;Transport=TCP;");
            }
            else if (radioButton_80Ton.Checked && radioButton_Output.Checked)
            {
                _connectionStringList.Add("172.20.200.7:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.8:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.15:80;Username=root;Password=49091;Transport=TCP;");
                _connectionStringList.Add("172.20.200.16:80;Username=root;Password=49091;Transport=TCP;");
            }
        }

        private void CreateVideoViewers()
        {
            _currentVideoViewer = videoViewerWF2;
            _videoViewerList.Add(videoViewerWF2);
            _videoViewerList.Add(videoViewerWF3);
            _videoViewerList.Add(videoViewerWF4);
            _videoViewerList.Add(videoViewerWF9);
        }

        private void CreateIPCameraHandlers()
        {
            var i = 0;
            while (i < 4)
            {
                ModelList.Add(new IpCameraHandler());
                i++;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var i = 0;
            while (i < _videoViewerList.Count)
            {
                _videoViewerList[i].SetImageProvider(ModelList[i].ImageProvider);
                i++;
            }

            foreach (var item in ModelList)
            {
                item.CameraStateChanged += ModelCameraStateChanged;
                item.CameraErrorOccurred += ModelCameraErrorOccurred;
            }

            //comboBox_Direction.DataSource = Enum.GetValues(typeof(PatrolDirection));

            //textBox_SaveTo1.Text = "C:\\";
            //textBox_SaveTo2.Text = "C:\\";
            //textBox_SaveTo3.Text = "C:\\";
            //ActiveCameraCombo.SelectedIndex = 0;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var item in ModelList)
                item.Stop();

            ModelList.Clear();
            ModelList = null;

            foreach (var viewer in _videoViewerList)
            {
                viewer.Stop();
                viewer.Dispose();
            }

            _videoViewerList.Clear();
            _videoViewerList = null;
        }

        private void ModelCameraStateChanged(object sender, CameraStateEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                IpCameraHandler cc = sender as IpCameraHandler;
                if(cc.Camera != null)
                    Log.Write(cc.Camera.Host + " -- Camera state: " +e.State);
                switch (e.State)
                {
                    case CameraState.Streaming:
                        button_Connect.Enabled = false;
                        button_Disconnect.Enabled = true;
                        _currentVideoViewer.Start();
                        //ClearFields();
                        GetCameraStreams(cc);

                        //if (cc.Camera.UriType != CameraUriType.RTSP)
                            //InitializeTrackBars();
                        break;

                    case CameraState.Disconnected:
                        button_Disconnect.Enabled = false;
                        _currentVideoViewer.Stop();
                        button_Connect.Enabled = true;
                        break;
                }
            });
        }

        private void GetCameraStreams(IpCameraHandler cameraHandler)
        {
            //if (cameraHandler.Camera != null)
            //    cameraHandler.Camera.Start();
            
        }

        private void ModelCameraErrorOccurred(object sender, CameraErrorEventArgs e)
        {
            InvokeGuiThread(() => 
                {
                    IpCameraHandler cc = sender as IpCameraHandler;
                    if (cc.Camera != null)
                        Log.Write(cc.Camera.Host + " -- Camera error: " + (e.Details ?? e.Error.ToString()));
                });
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = new LinkLabel.Link { LinkData = "http://www.camera-sdk.com/" };

            if (link.LinkData != null) Process.Start(link.LinkData.ToString());
        }

        #region Connect - Disconnect

        private void button_Connect_Click(object sender, EventArgs e)
        {
            CreateConnectionStrings();
            ConnectIpCam();
        }

        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            //if (ActiveCameraCombo.SelectedIndex != -1 && ModelList[ActiveCameraCombo.SelectedIndex].Camera != null)
            foreach (var item in ModelList)
            {
                item.Disconnect();
            }
           
            //ClearFields();
        }

        private void ConnectIpCam()
        {
            var i = 0;
            while (i < ModelList.Count)
            {
                if (ModelList[i] == null) return;
                ModelList[i].ConnectOnvifCamera(_connectionStringList[i]);
                _videoViewerList[i].Start();

                i++;
            }
        }

        #endregion

        #region LOG

        void Log_OnLogMessageReceived(object sender, LogEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                logListBox.Items.Add(e.LogMessage);
                LogScroll();
            });
        }

        void LogScroll()
        {
            logListBox.SelectedIndex = logListBox.Items.Count - 1;
            logListBox.SelectedIndex = -1;
        }

        #endregion

        //private void ClearFields()
        //{
        //    InvokeGuiThread(() =>
        //    {
        //        //StreamCombo.Items.Clear();
        //        //AudioInfoText.Clear();
        //        //VideoInfoText.Clear();
        //        //DetailsText.Clear();
        //       // StreamCombo.Text = String.Empty;
        //    });
        //}


        #region Image Settings


        #endregion


        private void InvokeGuiThread(Action action)
        {
            BeginInvoke(action);
        }

        private void button_CaptureImage(object sender, EventArgs e)
        {
            string path = "C:\\";
            foreach(var item in ModelList)
            {
                if(item == null || item.Camera == null) continue;
                var date = item.Camera.Host.ToString() + "-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                //var image = item.CreateSnapShot(path);
                byte[] image = null;
                if (image != null)
                {
                    Log.Write("Saving Picture " + item.Camera.Host + " To Database");

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO SIDev_Binary (BinaryTitle, BinaryPath, BinaryData, BinaryExt, BinarySize, CreatorID, AttachDate, Embedded, Guid)" +
                                                       "VALUES (@date, @date, @Image, '.jpg', @ImageSize, 1, GETDATE(), 1, NEWID())", _dbConnector);
                    sqlCommand.Parameters.AddWithValue("@date", date);
                    sqlCommand.Parameters.AddWithValue("@Image", image);
                    sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();

                    sqlCommand = new SqlCommand("SELECT ID, Guid FROM SIDev_Binary WHERE BinaryTitle = '" + date + "'", _dbConnector);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable BinaryTable = new DataTable();
                    sqlAdapter.Fill(BinaryTable);
                    sqlCommand.Dispose();

                    if(radioButton_Sales.Checked)
                    {
                        sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                                                           "VALUES (2631, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnector);
                    }
                    else if (radioButton_Purchase.Checked)
                    {
                        sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                                                           "VALUES (2987, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnector);
                    }
                    sqlCommand.Parameters.AddWithValue("@MainGuid", _shipmentTable.Rows[0].ItemArray[2].ToString());
                    sqlCommand.Parameters.AddWithValue("@RelatedGuid", BinaryTable.Rows[0].ItemArray[1].ToString());
                    sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();

                    Log.Write("Saving Picture Completed");
                }
            }
            
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (radioButton_Sales.Checked)
            {
                _dbCommand = new SqlCommand("SELECT  SDSO_Shipment.Title AS ShipmentTitle, SDSO_Shipment.TransportCode AS TransportCode, SDSO_Customer.Title AS Destination, WMLog_Vehicle.CarrierNumber, WMLog_Driver.Title AS DriverTitle, WMLog_Driver.LicenseNumber AS LicenseNumber, SDSO_Shipment.Guid " +
                                            "FROM SDSO_Shipment LEFT OUTER JOIN WMLog_Driver " +
                                            "ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WMLog_Vehicle " +
                                            "ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN SDSO_Customer " +
                                            "ON SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode WHERE SDSO_Shipment.Code = '" + txt_Search.Text + "'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                _shipmentTable = new DataTable();
                _dbAdapter.Fill(_shipmentTable);
                if (_shipmentTable.Rows.Count > 0)
                {
                    txt_ShipmentTitle.Text = _shipmentTable.Rows[0].ItemArray[0].ToString();
                    txt_TransportCode.Text = _shipmentTable.Rows[0].ItemArray[1].ToString();
                    txt_Destination.Text = _shipmentTable.Rows[0].ItemArray[2].ToString();
                    txt_Car.Text = _shipmentTable.Rows[0].ItemArray[3].ToString();
                    txt_Driver.Text = _shipmentTable.Rows[0].ItemArray[4].ToString();
                    txt_LicenseCode.Text = _shipmentTable.Rows[0].ItemArray[5].ToString();
                }
                else
                {
                    txt_ShipmentTitle.Text = "";
                    txt_Driver.Text = "";
                    txt_TransportCode.Text = "";
                    txt_Destination.Text = "";
                    txt_Car.Text = "";
                    txt_LicenseCode.Text = "";
                }

                _dbCommand = new SqlCommand("SELECT  Sequence as ردیف, PartSerialCode as [بارکد شمش], ShipmentAuthorizeCode as [مجوز حمل], ProductCode as [کد کالا], Quantity as مقدار, UnitOfMeasureCode as [واحد اندازه گیری] FROM SDSO_Shipment INNER JOIN SDSO_ShipmentDetail ON SDSO_Shipment.Code = SDSO_ShipmentDetail.ShipmentCode WHERE Code = '" + txt_Search.Text + "'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                DataTable shipmentDetailTable = new DataTable();
                _dbAdapter.Fill(shipmentDetailTable);
                if (shipmentDetailTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = shipmentDetailTable;
                }
            }
            else if (radioButton_Purchase.Checked)
            {
                _dbCommand = new SqlCommand("SELECT  SMPO_Shipment.Title AS ShipmentTitle, WMLog_Driver.Title AS DriverTitle, SMPO_Shipment.Guid FROM SMPO_Shipment LEFT OUTER JOIN WMLog_Driver ON SMPO_Shipment.DriverCode = WMLog_Driver.DriverCode WHERE Code = '" + txt_Search.Text + "'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                _shipmentTable = new DataTable();
                _dbAdapter.Fill(_shipmentTable);
                if (_shipmentTable.Rows.Count > 0)
                {
                    txt_ShipmentTitle.Text = _shipmentTable.Rows[0].ItemArray[0].ToString();
                    txt_Driver.Text = _shipmentTable.Rows[0].ItemArray[1].ToString();
                }
                else
                {
                    txt_ShipmentTitle.Text = "";
                    txt_Driver.Text = "";
                }

                _dbCommand = new SqlCommand("SELECT  Sequence as ردیف, ReceiveCode as [مجوز حمل], PartCode as [کد کالا], Quantity as مقدار, UnitOfMeasureCode as [واحد اندازه گیری] FROM SMPO_Shipment INNER JOIN SMPO_ShipmentDetail ON SMPO_Shipment.Code = SMPO_ShipmentDetail.ShipmentCode WHERE Code = '" + txt_Search.Text + "'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                DataTable shipmentDetailTable = new DataTable();
                _dbAdapter.Fill(shipmentDetailTable);
                if (shipmentDetailTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = shipmentDetailTable;
                }
            }
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Search.Text))
                btn_Search.Enabled = false;
            else
                btn_Search.Enabled = true;
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btn_Search_Click(sender, e);
            }
        }

        private void button_FirstWeighing_Click(object sender, EventArgs e)
        {
            SerialPort serialPort = new System.IO.Ports.SerialPort();
            serialPort.PortName = "COM3";
            serialPort.BaudRate = 2400;
            serialPort.DataBits = 8;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.Handshake = System.IO.Ports.Handshake.None;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.RtsEnable = true;
            serialPort.Encoding = Encoding.ASCII;

            SerialPort serialPort2 = new System.IO.Ports.SerialPort();
            serialPort2.PortName = "COM4";
            serialPort2.BaudRate = 2400;
            serialPort2.DataBits = 8;
            serialPort2.Parity = System.IO.Ports.Parity.None;
            serialPort2.Handshake = System.IO.Ports.Handshake.None;
            serialPort2.StopBits = System.IO.Ports.StopBits.One;
            serialPort2.RtsEnable = true;
            serialPort2.Encoding = Encoding.ASCII;

            try
            {
                serialPort.Open();
                serialPort2.Open();
                byte[] v = new byte[8];
                int tryCount = 0;

                Log.Write(serialPort.BytesToRead + " " + serialPort2.BytesToRead);

                List<int> results = new List<int>();
                while (serialPort.BytesToRead > 0 && tryCount < 10)
                {

                    var output = serialPort.Read(v, 0, 7);

                    if (output > 0)
                    {
                        try
                        {
                            int intResult = Int32.Parse(System.Text.Encoding.ASCII.GetString(v, 1, 6));
                            results.Add(intResult);
                            tryCount++;
                        }
                        catch (FormatException exp)
                        {
                            tryCount++;
                        }
                    }
                    else
                        tryCount++;
                }
                Log.Write("PU800 on COM3 is connected.\n Weight: " + results.GroupBy(i => i).OrderByDescending(grp => grp.Count()).First().ToString());

                tryCount = 0;
                results.Clear();
                while (serialPort2.BytesToRead > 0 && tryCount < 10)
                {
                    var output = serialPort2.Read(v, 0, 7);
                    if (output > 0)
                    {
                        try
                        {
                            int intResult = Int32.Parse(System.Text.Encoding.ASCII.GetString(v, 1, 6));
                            results.Add(intResult);
                            tryCount++;
                        }
                        catch (FormatException exp)
                        {
                            tryCount++;
                        }
                    }
                    else
                        tryCount++;
                }
                Log.Write("PU800 on COM4 is connected.\n Weight: " + results.GroupBy(i => i).OrderByDescending(grp => grp.Count()).First().ToString());

                serialPort.Close();
                serialPort2.Close();
            }
            catch (Exception)
            {
                Log.Write("Error Opening Connection to Weighing Machine.");
            }
        }

        private void button_SecondWeighing_Click(object sender, EventArgs e)
        {
            Log.Write("Error Opening Connection to Weighing Machine.");
        }
        
    }
}
