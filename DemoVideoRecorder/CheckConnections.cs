using _03_Onvif_Network_Video_Recorder.Properties;
using Ozeki.Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class CheckConnections : Form
    {
        private List<string> _connectionStringList;
        private List<IpCameraHandler> ModelList;
        private List<PictureBox> _indicatorList;
        public CheckConnections()
        {
            InitializeComponent();
            _connectionStringList = new List<string>();
            ModelList = new List<IpCameraHandler>();
            _indicatorList = new List<PictureBox>();
            CreateIPCameraHandlers();
            CreateIndicators();
            CreateCheckLabels();
        }

        private void CreateCheckLabels()
        {
            lblCamera1.Text = "آدرس دوربین " + Settings.Default.CameraIP1;
            lblCamera2.Text = "آدرس دوربین " + Settings.Default.CameraIP2;
            lblCamera3.Text = "آدرس دوربین " + Settings.Default.CameraIP3;
            lblCamera4.Text = "آدرس دوربین " + Settings.Default.CameraIP4;
            lblBascol.Text = "پورت باسکول " + Settings.Default.BascolPort;
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
                    lblCamera1.Text = "اتصال به دوربین " + cameraAddress[0] + " با خطا مواجه شد!";
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP2)
                {
                    _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                    lblCamera2.Text = "اتصال به دوربین " + cameraAddress[0] + " با خطا مواجه شد!";
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP3)
                {
                    _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                    lblCamera3.Text = "اتصال به دوربین " + cameraAddress[0] + " با خطا مواجه شد!";
                }
                else if (cameraAddress[0] == Settings.Default.CameraIP4)
                {
                    _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
                    lblCamera4.Text = "اتصال به دوربین " + cameraAddress[0] + " با خطا مواجه شد!";
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
                            lblCamera1.Text = "با موفقیت به دوربین " + cameraAddress[0] + " متصل شد.";
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP2)
                        {
                            _indicatorList[1].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                            lblCamera2.Text = "با موفقیت به دوربین " + cameraAddress[0] + " متصل شد.";
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP3)
                        {
                            _indicatorList[2].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                            lblCamera3.Text = "با موفقیت به دوربین " + cameraAddress[0] + " متصل شد.";
                        }
                        else if (cameraAddress[0] == Settings.Default.CameraIP4)
                        {
                            _indicatorList[3].Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                            lblCamera4.Text = "با موفقیت به دوربین " + cameraAddress[0] + " متصل شد.";
                        }
                        cc.Camera.Disconnect();
                        break;

                    case CameraState.Connecting:
                        if (cameraAddress[0] == Settings.Default.CameraIP1)
                            lblCamera1.Text = "در حال بررسی اتصال دوربین " + cameraAddress[0] + "...";
                        else if (cameraAddress[0] == Settings.Default.CameraIP2)
                            lblCamera2.Text = "در حال بررسی اتصال دوربین " + cameraAddress[0] + "...";
                        else if (cameraAddress[0] == Settings.Default.CameraIP3)
                            lblCamera3.Text = "در حال بررسی اتصال دوربین " + cameraAddress[0] + "...";
                        else if (cameraAddress[0] == Settings.Default.CameraIP4)
                            lblCamera4.Text = "در حال بررسی اتصال دوربین " + cameraAddress[0] + "...";
                        break;
                }
            });
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
            imgBascol.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.yellow);
            SerialPort serialPort = new System.IO.Ports.SerialPort();
            serialPort.PortName = Settings.Default.BascolPort;
            serialPort.BaudRate = 2400;
            serialPort.DataBits = 8;
            serialPort.Parity = System.IO.Ports.Parity.None;
            serialPort.Handshake = System.IO.Ports.Handshake.None;
            serialPort.StopBits = System.IO.Ports.StopBits.One;
            serialPort.RtsEnable = true;
            serialPort.Encoding = Encoding.ASCII;

            serialPort.Open();
            byte[] v = new byte[8];
            int tryCount = 0;

            lblBascol.Text = "در حال بررسی اتصال به باسکول " + Settings.Default.BascolPort + " ...";
            if (serialPort.BytesToRead <= 0)
            {
                lblBascol.Text = "خطا در اتصال به باسکول " + Settings.Default.BascolPort;
                imgBascol.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
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
                            lblBascol.Text = "با موفقیت به باسکول " + Settings.Default.BascolPort + " متصل شد.";
                            imgBascol.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.green);
                            tryCount = 10;
                        }
                        catch (FormatException exp)
                        {
                            lblBascol.Text = "خطا در اتصال به باسکول " + Settings.Default.BascolPort;
                            imgBascol.Image = new Bitmap(_03_Onvif_Network_Video_Recorder.Properties.Resources.red);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
