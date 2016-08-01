using _03_Onvif_Network_Video_Recorder.Properties;
using Ozeki.Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class Weighing : Form
    {
        private List<string> _connectionStringList;
        private List<IpCameraHandler> ModelList;
        private List<PictureBox> _indicatorList;
        private SerialPort _serialPort;
        private string _shipmentState;
        public Weighing()
        {
            InitializeComponent();
            this.Load += Configuration_Load;
            this.FormClosing += Weighing_FormClosing;
        }

        void Weighing_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(var camera in ModelList)
            {
                if (camera.Camera != null && camera.Camera.State != CameraState.Disconnected)
                    camera.Camera.Disconnect();
            }
            if(_serialPort.IsOpen)
                _serialPort.Close();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            _connectionStringList = new List<string>();
            ModelList = new List<IpCameraHandler>();
            _indicatorList = new List<PictureBox>();
            CreateIPCameraHandlers();
            CreateIndicators();
            CreateConnectionStrings();
            CreateSerialPort();
        }

        private void CreateSerialPort()
        {
            WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
            _serialPort = new SerialPort();
            _serialPort.PortName = Settings.Default.BascolPort;
            _serialPort.BaudRate = 2400;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.RtsEnable = true;
            _serialPort.Encoding = Encoding.ASCII;

            _serialPort.DataReceived +=
                new SerialDataReceivedEventHandler(_serialPort_DataReceived);
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = _serialPort.BytesToRead;
            byte[] data = new byte[dataLength];
            int nbrDataRead = _serialPort.Read(data, 0, dataLength);
            if (nbrDataRead == 0)
                return;

            // Send data to whom ever interested
            txtTest1.Text = Convert.ToBase64String(data);
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
                if (InvokeRequired)
                {
                    BeginInvoke(action);
                }
            }
        }
        private void CreateIPCameraHandlers()
        {
            ModelList.Clear();
            var i = 0;
            while (i < 4)
            {
                ModelList.Add(new IpCameraHandler());
                i++;
            }
            foreach (var item in ModelList)
            {
                item.CameraStateChanged += ModelCameraStateChanged;
                item.CameraErrorOccurred += ModelCameraErrorOccurred;
            }
        }

        private void ModelCameraErrorOccurred(object sender, Ozeki.Camera.CameraErrorEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                IpCameraHandler cc = sender as IpCameraHandler;
                var cameraAddress = cc.Camera.CameraAddress.Split(':');
                if (cameraAddress[0] == Settings.Default.CameraIP1)
                {
                    _indicatorList[0].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP2)
                {
                    _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP3)
                {
                    _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP4)
                {
                    _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                }
            });
        }

        private void ModelCameraStateChanged(object sender, Ozeki.Camera.CameraStateEventArgs e)
        {
            InvokeGuiThread(() =>
            {
                IpCameraHandler cc = sender as IpCameraHandler;
                var cameraAddress = cc.Camera.CameraAddress.Split(':');
                switch (e.State)
                {
                    case CameraState.Connected:
                        if (cameraAddress[0] == Settings.Default.CameraIP1)
                        {
                            _indicatorList[0].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP2)
                        {
                            _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP3)
                        {
                            _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP4)
                        {
                            _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                        }
                        break;

                    case CameraState.Disconnected:
                        if (cameraAddress[0] == Settings.Default.CameraIP1)
                        {
                            _indicatorList[0].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP2)
                        {
                            _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP3)
                        {
                            _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP4)
                        {
                            _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                        }
                        break;

                    case CameraState.Connecting:
                        if (cameraAddress[0] == Settings.Default.CameraIP1)
                            _indicatorList[0].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
                        else if (cameraAddress[0] == Settings.Default.CameraIP2)
                            _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
                        else if (cameraAddress[0] == Settings.Default.CameraIP3)
                            _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
                        else if (cameraAddress[0] == Settings.Default.CameraIP4)
                            _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
                        break;
                }
            });
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnShipmentSearch_Click(object sender, EventArgs e)
        {
            DatabaseIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
            using (SqlConnection con = new SqlConnection("Data Source=tcp:127.0.0.1;initial catalog=AshaMES_PASCO_V03;persist security info=True;user id=sa;password=@sh@3rp;MultipleActiveResultSets=True;"))
            {
                DatabaseIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                using (SqlCommand cmd = new SqlCommand("SELECT SDSO_Shipment.Title AS ShipmentTitle, SDSO_Shipment.TransportCode AS TransportCode, SDSO_Customer.Title AS Destination, WMLog_Vehicle.CarrierNumber, WMLog_Driver.Title AS DriverTitle, WMLog_Driver.LicenseNumber AS LicenseNumber, SDSO_Shipment.Guid, SDSO_Shipment.FormStatusCode AS ShipmentStatus " +
                                                        "FROM SDSO_Shipment LEFT OUTER JOIN WMLog_Driver " +
                                                        "ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WMLog_Vehicle " +
                                                        "ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN SDSO_Customer " +
                                                        "ON SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode WHERE SDSO_Shipment.FormStatusCode Like '%Weighing%' AND SDSO_Shipment.Code LIKE '%" + txtShipmentCode.Text + "%'"
                                                        , con))
                {   
                    con.Open();
                    DataTable _shipmentTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(_shipmentTable);
                    if (_shipmentTable.Rows.Count > 0)
                    {
                        lblSource.Text = _shipmentTable.Rows[0].ItemArray[0].ToString();
                        lblBillOfLading.Text = _shipmentTable.Rows[0].ItemArray[1].ToString();
                        lblDestination.Text = _shipmentTable.Rows[0].ItemArray[2].ToString();
                        lblCar.Text = _shipmentTable.Rows[0].ItemArray[3].ToString();
                        lblDriver.Text = _shipmentTable.Rows[0].ItemArray[4].ToString();
                        lblDriverLicence.Text = _shipmentTable.Rows[0].ItemArray[5].ToString();
                        _shipmentState = _shipmentTable.Rows[0].ItemArray[7].ToString();
                    }
                    else
                    {
                        lblSource.Text = "";
                        lblDriver.Text = "";
                        lblBillOfLading.Text = "";
                        lblDestination.Text = "";
                        lblCar.Text = "";
                        lblDriverLicence.Text = "";
                    }
                }
            }

            using (SqlConnection con = new SqlConnection("Data Source=tcp:127.0.0.1;initial catalog=AshaMES_PASCO_V03;persist security info=True;user id=sa;password=@sh@3rp;MultipleActiveResultSets=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT  Sequence as ردیف, PartSerialCode as [بارکد شمش], ProductCode as [کد کالا], WMInv_Part.Title as [نام کالا], ShipmentAuthorizeCode as [مجوز حمل] FROM SDSO_Shipment " +
                    "INNER JOIN SDSO_ShipmentDetail ON SDSO_Shipment.Code = SDSO_ShipmentDetail.ShipmentCode " +
                    "INNER JOIN WMInv_Part ON SDSO_ShipmentDetail.ProductCode = WMInv_Part.Code WHERE SDSO_Shipment.FormStatusCode Like '%Weighing%' AND SDSO_Shipment.Code Like '" + txtShipmentCode.Text + "'"
                                                        , con))
                {
                    con.Open();
                    DataTable shipmentDetailTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(shipmentDetailTable);
                    if (shipmentDetailTable.Rows.Count > 0)
                    {
                        dgShipmentDetail.DataSource = shipmentDetailTable;
                    }
                }
            }
            DatabaseIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
        }

        private void txtShipmentCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnShipmentSearch_Click(sender, e);
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            string path = "C:\\";
            for (int i = 0; i < 4; i++)
            {
                if (ModelList[i] == null || ModelList[i].Camera == null) continue;
                var date = ModelList[i].Camera.Host.ToString() + "-" + DateTime.Now.Year + "y-" + DateTime.Now.Month + "m-" + DateTime.Now.Day + "d-" +
                       DateTime.Now.Hour + "h-" + DateTime.Now.Minute + "m-" + DateTime.Now.Second + "s.jpg";
                var image = ModelList[i].CreateSnapShot(path);

                if (image != null)
                {
                    using (var ms = new System.IO.MemoryStream(image))
                    {
                        switch (i)
                        {
                            case 0:
                                imgCamera1.Image = new Bitmap(ms);
                                break;
                            case 1:
                                imgCamera2.Image = new Bitmap(ms);
                                break;
                            case 2:
                                imgCamera3.Image = new Bitmap(ms);
                                break;
                            case 3:
                                imgCamera4.Image = new Bitmap(ms);
                                break;
                        }
                    }
                }
            }

            if (_shipmentState == "Shp_FirstWeighing")
            {
                txtWeight1.Text = GetWeight();
                txtDate1.Text = GetDate();
                txtTime1.Text = GetTime();
            }
            else if (_shipmentState == "Shp_SecondWeighing")
            {
                txtWeight2.Text = GetWeight();
                txtDate2.Text = GetDate();
                txtTime2.Text = GetTime();
            }
        }

        private string GetWeight()
        {
            WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);

            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                    WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                }
                catch (Exception e)
                {
                    WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                    return "0";
                }
            }

            byte[] v = new byte[8];
            int intResult = 0;
            int tryCount = 0;

            if (_serialPort.BytesToRead <= 0)
            {
                WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
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
                            WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                            tryCount = 10;
                        }
                        catch (FormatException exp)
                        {
                            WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                            tryCount++;
                        }
                    }
                    else
                        tryCount++;
                }
            }

            
            return intResult.ToString();
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
            return string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
        }

        private void CreateConnectionStrings()
        {
            _connectionStringList.Clear();
            _connectionStringList.Add(Settings.Default.CameraIP1 + ":80;Username=root;Password=49091;Transport=TCP;");
            _connectionStringList.Add(Settings.Default.CameraIP2 + ":80;Username=root;Password=49091;Transport=TCP;");
            _connectionStringList.Add(Settings.Default.CameraIP3 + ":80;Username=root;Password=49091;Transport=TCP;");
            _connectionStringList.Add(Settings.Default.CameraIP4 + ":80;Username=root;Password=49091;Transport=TCP;");
        }

        private void ConnectIpCam()
        {
            var i = 0;
            while (i < ModelList.Count)
            {
                if (ModelList[i] == null && ModelList[i].Camera.State == CameraState.Connected) return;
                ModelList[i].ConnectOnvifCamera(_connectionStringList[i]);
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectIpCam();
            ConnectWeighingMachine();
        }

        private void ConnectWeighingMachine()
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                    WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                }
                catch (Exception e)
                {
                    WeighingMachineIndicator.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisconnectIpCam();
            DisconnectWeighingMachine();
        }

        private void DisconnectWeighingMachine()
        {
            if (_serialPort.IsOpen())
                _serialPort.Close();
        }

        private void DisconnectIpCam()
        {
            var i = 0;
            while (i < ModelList.Count)
            {
                if ((ModelList[i] == null || ModelList[i].Camera == null) && ModelList[i].Camera.State == CameraState.Disconnected) return;
                ModelList[i].Camera.Disconnect();
                i++;
            }
        }
    }
}
