using AshaWeighing.Properties;
//using Ozeki.Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AshaWeighing
{
    public partial class CheckConnections : Form
    {
        private List<string> _connectionStringList;
        private List<PictureBox> _indicatorList;
        public CheckConnections()
        {
            InitializeComponent();
            _connectionStringList = new List<string>();
            _indicatorList = new List<PictureBox>();
            CreateIPCameraHandlers();
            CreateIndicators();
            CreateCheckLabels();
        }

        private void CreateCheckLabels()
        {
            //lblCamera1.Text = "آدرس دوربین " + Settings.Default.CameraIP11;
            //lblCamera2.Text = "آدرس دوربین " + Settings.Default.CameraIP12;
            //lblCamera3.Text = "آدرس دوربین " + Settings.Default.CameraIP13;
            //lblCamera4.Text = "آدرس دوربین " + Settings.Default.CameraIP14;
            //lblBascol.Text = "پورت باسکول " + Settings.Default.BascolPort1;
        }

        private void CreateIndicators()
        {
            _indicatorList.Clear();
            _indicatorList.Add(imgCamera1);
            _indicatorList.Add(imgCamera2);
            _indicatorList.Add(imgCamera3);
            _indicatorList.Add(imgCamera4);
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
            
        }

        private void CheckConnections_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateConnectionStrings();
            ConnectIpCam();
            ConnectBascol();
        }

        private void ConnectBascol()
        {
            imgBascol.Image = new Bitmap(AshaWeighing.Properties.Resources.yellow);
            SerialPort serialPort = new System.IO.Ports.SerialPort();
            serialPort.PortName = Globals.WeighingMachineSerialPort;
            serialPort.BaudRate = 2400;
            serialPort.DataBits = 8;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.Handshake = System.IO.Ports.Handshake.None;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.RtsEnable = true;
            serialPort.Encoding = Encoding.ASCII;

            try
            {
                serialPort.Open();
            }
            catch(Exception)
            {
                lblBascol.Text = "خطا در اتصال به باسکول " + Globals.WeighingMachineSerialPort;
                imgBascol.Image = new Bitmap(AshaWeighing.Properties.Resources.red);
                return;
            }

            byte[] v = new byte[8];
            int tryCount = 0;

            lblBascol.Text = "در حال بررسی اتصال به باسکول " + Globals.WeighingMachineSerialPort + " ...";
            if (serialPort.BytesToRead <= 0)
            {
                lblBascol.Text = "خطا در خواندن اطلاعات " + Globals.WeighingMachineSerialPort;
                imgBascol.Image = new Bitmap(AshaWeighing.Properties.Resources.red);
            }
            else
            {
                while (serialPort.BytesToRead > 0 && tryCount < 10)
                {

                    var output = serialPort.Read(v, 0, 7);

                    if (output > 0)
                    {
                        try
                        {
                            int intResult = Int32.Parse(System.Text.Encoding.ASCII.GetString(v, 1, 6));
                            lblBascol.Text = "با موفقیت به باسکول " + Globals.WeighingMachineSerialPort + " متصل شد.";
                            imgBascol.Image = new Bitmap(AshaWeighing.Properties.Resources.green);
                            tryCount = 10;
                        }
                        catch (FormatException)
                        {
                            lblBascol.Text = "خطا در تشخیص اطلاعات " + Globals.WeighingMachineSerialPort;
                            imgBascol.Image = new Bitmap(AshaWeighing.Properties.Resources.red);
                            tryCount++;
                        }
                    }
                    else
                        tryCount++;
                }
            }

            serialPort.Close();
        }

        private void CreateConnectionStrings()
        {
            _connectionStringList.Clear();
            _connectionStringList.Add(Globals.CameraAddress[0] + ":80;Username=" + Globals.CameraUsername[0] + ";Password=" + Globals.CameraPassword[0] + ";Transport=TCP;");
            _connectionStringList.Add(Globals.CameraAddress[1] + ":80;Username=" + Globals.CameraUsername[1] + ";Password=" + Globals.CameraPassword[1] + ";Transport=TCP;");
            _connectionStringList.Add(Globals.CameraAddress[2] + ":80;Username=" + Globals.CameraUsername[2] + ";Password=" + Globals.CameraPassword[2] + ";Transport=TCP;");
            _connectionStringList.Add(Globals.CameraAddress[3] + ":80;Username=" + Globals.CameraUsername[3] + ";Password=" + Globals.CameraPassword[3] + ";Transport=TCP;");
        }

        private void ConnectIpCam()
        {
            var i = 0;
            i = 0;
            while (i < _indicatorList.Count)
            {
                if (_indicatorList[i] == null) return;
                _indicatorList[i].Image = new Bitmap(AshaWeighing.Properties.Resources.yellow);

                i++;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
