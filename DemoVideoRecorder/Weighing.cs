using _03_Onvif_Network_Video_Recorder.Properties;
using Ozeki.Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
        public Weighing()
        {
            InitializeComponent();
            this.Load += Configuration_Load;
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            _connectionStringList = new List<string>();
            ModelList = new List<IpCameraHandler>();
            _indicatorList = new List<PictureBox>();
            CreateIPCameraHandlers();
            CreateIndicators();
            CreateConnectionStrings();
            ConnectIpCam();
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
                //if (cc.Camera != null)
                //    Log.Write(cc.Camera.Host + " -- Camera state: " + e.State);
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
            using (SqlConnection con = new SqlConnection("Data Source=tcp:127.0.0.1;initial catalog=AshaMES_PASCO_V03;persist security info=True;user id=sa;password=@sh@3rp;MultipleActiveResultSets=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT  SDSO_Shipment.Title AS ShipmentTitle, SDSO_Shipment.TransportCode AS TransportCode, SDSO_Customer.Title AS Destination, WMLog_Vehicle.CarrierNumber, WMLog_Driver.Title AS DriverTitle, WMLog_Driver.LicenseNumber AS LicenseNumber, SDSO_Shipment.Guid " +
                                                        "FROM SDSO_Shipment LEFT OUTER JOIN WMLog_Driver " +
                                                        "ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WMLog_Vehicle " +
                                                        "ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN SDSO_Customer " +
                                                        "ON SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode WHERE SDSO_Shipment.Code LIKE '%" + txtShipmentCode.Text + "%'"
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
                using (SqlCommand cmd = new SqlCommand("SELECT  Sequence as ردیف, PartSerialCode as [بارکد شمش], ShipmentAuthorizeCode as [مجوز حمل], ProductCode as [کد کالا], Quantity as مقدار, UnitOfMeasureCode as [واحد اندازه گیری] FROM SDSO_Shipment INNER JOIN SDSO_ShipmentDetail ON SDSO_Shipment.Code = SDSO_ShipmentDetail.ShipmentCode WHERE Code = '" + txtShipmentCode.Text + "'"
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
                    //SqlCommand sqlCommand = new SqlCommand("INSERT INTO SIDev_Binary (BinaryTitle, BinaryPath, BinaryData, BinaryExt, BinarySize, CreatorID, AttachDate, Embedded, Guid)" +
                    //                                   "VALUES (@date, @date, @Image, '.jpg', @ImageSize, 1, GETDATE(), 1, NEWID())", _dbConnector);
                    //sqlCommand.Parameters.AddWithValue("@date", date);
                    //sqlCommand.Parameters.AddWithValue("@Image", image);
                    //sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                    //sqlCommand.ExecuteNonQuery();
                    //sqlCommand.Dispose();

                    //sqlCommand = new SqlCommand("SELECT ID, Guid FROM SIDev_Binary WHERE BinaryTitle = '" + date + "'", _dbConnector);
                    //SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    //DataTable BinaryTable = new DataTable();
                    //sqlAdapter.Fill(BinaryTable);
                    //sqlCommand.Dispose();

                    //if (radioButton_Sales.Checked)
                    //{
                    //    sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                    //                                       "VALUES (2631, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnector);
                    //}
                    //else if (radioButton_Purchase.Checked)
                    //{
                    //    sqlCommand = new SqlCommand("INSERT INTO SIDev_Attachment (MainSysEntityID, RelatedSysEntityID, MainItemGuid, RelatedItemGuid, AttachmentType)" +
                    //                                       "VALUES (2987, 2822, @MainGuid, @RelatedGuid, 2)", _dbConnector);
                    //}
                    //sqlCommand.Parameters.AddWithValue("@MainGuid", _shipmentTable.Rows[0].ItemArray[2].ToString());
                    //sqlCommand.Parameters.AddWithValue("@RelatedGuid", BinaryTable.Rows[0].ItemArray[1].ToString());
                    //sqlCommand.Parameters.AddWithValue("@ImageSize", image.Length);
                    //sqlCommand.ExecuteNonQuery();
                    //sqlCommand.Dispose();

                    //Log.Write("Saving Picture Completed");
                }
            }
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
                if (ModelList[i] == null) return;
                ModelList[i].ConnectOnvifCamera(_connectionStringList[i]);
                //_videoViewerList[i].Start();

                i++;
            }
            i = 0;
            while (i < _indicatorList.Count)
            {
                if (_indicatorList[i] == null) return;
                _indicatorList[i].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
                //_videoViewerList[i].Start();

                i++;
            }
        }
    }
}
