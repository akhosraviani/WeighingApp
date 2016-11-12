namespace _03_Onvif_Network_Video_Recorder
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

      
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.button_Disconnect = new System.Windows.Forms.Button();
            this.button_CaptureImage1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.CameraBox1 = new System.Windows.Forms.GroupBox();
            //this.videoViewerWF2 = new Ozeki.Media.VideoViewerWF();
            this.CameraBox2 = new System.Windows.Forms.GroupBox();
            this.videoViewerWF3 = new Ozeki.Media.VideoViewerWF();
            this.CameraBox3 = new System.Windows.Forms.GroupBox();
            this.videoViewerWF4 = new Ozeki.Media.VideoViewerWF();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.videoViewerWF1 = new Ozeki.Media.VideoViewerWF();
            this.videoViewerWF5 = new Ozeki.Media.VideoViewerWF();
            this.videoViewerWF6 = new Ozeki.Media.VideoViewerWF();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.videoViewerWF9 = new Ozeki.Media.VideoViewerWF();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Source = new System.Windows.Forms.TextBox();
            this.txt_Destination = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Receiver = new System.Windows.Forms.TextBox();
            this.txt_Sender = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_ShipmentTitle = new System.Windows.Forms.TextBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button_FirstWeighing = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radioButton_Purchase = new System.Windows.Forms.RadioButton();
            this.radioButton_Sales = new System.Windows.Forms.RadioButton();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.radioButton_80Ton = new System.Windows.Forms.RadioButton();
            this.radioButton_60Ton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_Output = new System.Windows.Forms.RadioButton();
            this.radioButton_Input = new System.Windows.Forms.RadioButton();
            this.videoViewerWF7 = new Ozeki.Media.VideoViewerWF();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txt_NetWeight = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_FullWeight = new System.Windows.Forms.TextBox();
            this.txt_EmptyWeight = new System.Windows.Forms.TextBox();
            this.txt_TransportCode = new System.Windows.Forms.TextBox();
            this.txt_Car = new System.Windows.Forms.TextBox();
            this.txt_Driver = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_DriverID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_LicenseCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.CameraBox1.SuspendLayout();
            this.CameraBox2.SuspendLayout();
            this.CameraBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Connect);
            this.groupBox1.Controls.Add(this.button_Disconnect);
            this.groupBox1.Location = new System.Drawing.Point(1272, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(208, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "اتصال دوربین ها";
            // 
            // button_Connect
            // 
            this.button_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Connect.ForeColor = System.Drawing.Color.Black;
            this.button_Connect.Location = new System.Drawing.Point(7, 28);
            this.button_Connect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(98, 58);
            this.button_Connect.TabIndex = 34;
            this.button_Connect.Text = "برقراری اتصال";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // button_Disconnect
            // 
            this.button_Disconnect.Enabled = false;
            this.button_Disconnect.Location = new System.Drawing.Point(112, 28);
            this.button_Disconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_Disconnect.Name = "button_Disconnect";
            this.button_Disconnect.Size = new System.Drawing.Size(85, 58);
            this.button_Disconnect.TabIndex = 38;
            this.button_Disconnect.Text = "قطع اتصال";
            this.button_Disconnect.UseVisualStyleBackColor = true;
            this.button_Disconnect.Click += new System.EventHandler(this.button_Disconnect_Click);
            // 
            // button_CaptureImage1
            // 
            this.button_CaptureImage1.Location = new System.Drawing.Point(124, 28);
            this.button_CaptureImage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_CaptureImage1.Name = "button_CaptureImage1";
            this.button_CaptureImage1.Size = new System.Drawing.Size(94, 37);
            this.button_CaptureImage1.TabIndex = 17;
            this.button_CaptureImage1.Text = "Capture image";
            this.button_CaptureImage1.UseVisualStyleBackColor = true;
            this.button_CaptureImage1.Click += new System.EventHandler(this.button_CaptureImage);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.logListBox);
            this.groupBox3.Location = new System.Drawing.Point(889, 127);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(590, 83);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Event log";
            // 
            // logListBox
            // 
            this.logListBox.FormattingEnabled = true;
            this.logListBox.HorizontalScrollbar = true;
            this.logListBox.ItemHeight = 19;
            this.logListBox.Location = new System.Drawing.Point(7, 25);
            this.logListBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(572, 42);
            this.logListBox.TabIndex = 0;
            // 
            // CameraBox1
            // 
            this.CameraBox1.Controls.Add(this.videoViewerWF2);
            this.CameraBox1.Location = new System.Drawing.Point(889, 219);
            this.CameraBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox1.Name = "CameraBox1";
            this.CameraBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CameraBox1.Size = new System.Drawing.Size(292, 292);
            this.CameraBox1.TabIndex = 3;
            this.CameraBox1.TabStop = false;
            this.CameraBox1.Text = "دوربین 1";
            // 
            // videoViewerWF2
            // 
            this.videoViewerWF2.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF2.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF2.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF2.FullScreenEnabled = true;
            this.videoViewerWF2.Location = new System.Drawing.Point(7, 28);
            this.videoViewerWF2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.videoViewerWF2.Name = "videoViewerWF2";
            this.videoViewerWF2.RotateAngle = 0;
            this.videoViewerWF2.Size = new System.Drawing.Size(278, 256);
            this.videoViewerWF2.TabIndex = 0;
            this.videoViewerWF2.Text = "videoViewerWF2";
            // 
            // CameraBox2
            // 
            this.CameraBox2.Controls.Add(this.videoViewerWF3);
            this.CameraBox2.Location = new System.Drawing.Point(1188, 219);
            this.CameraBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox2.Name = "CameraBox2";
            this.CameraBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CameraBox2.Size = new System.Drawing.Size(292, 292);
            this.CameraBox2.TabIndex = 15;
            this.CameraBox2.TabStop = false;
            this.CameraBox2.Text = "دوربین 2";
            // 
            // videoViewerWF3
            // 
            this.videoViewerWF3.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF3.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF3.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF3.FullScreenEnabled = true;
            this.videoViewerWF3.Location = new System.Drawing.Point(7, 28);
            this.videoViewerWF3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.videoViewerWF3.Name = "videoViewerWF3";
            this.videoViewerWF3.RotateAngle = 0;
            this.videoViewerWF3.Size = new System.Drawing.Size(278, 256);
            this.videoViewerWF3.TabIndex = 1;
            this.videoViewerWF3.Text = "videoViewerWF3";
            // 
            // CameraBox3
            // 
            this.CameraBox3.Controls.Add(this.videoViewerWF4);
            this.CameraBox3.Location = new System.Drawing.Point(889, 522);
            this.CameraBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox3.Name = "CameraBox3";
            this.CameraBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CameraBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CameraBox3.Size = new System.Drawing.Size(292, 292);
            this.CameraBox3.TabIndex = 16;
            this.CameraBox3.TabStop = false;
            this.CameraBox3.Text = "دوربین 3";
            // 
            // videoViewerWF4
            // 
            this.videoViewerWF4.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF4.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF4.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF4.FullScreenEnabled = true;
            this.videoViewerWF4.Location = new System.Drawing.Point(7, 28);
            this.videoViewerWF4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.videoViewerWF4.Name = "videoViewerWF4";
            this.videoViewerWF4.RotateAngle = 0;
            this.videoViewerWF4.Size = new System.Drawing.Size(278, 256);
            this.videoViewerWF4.TabIndex = 2;
            this.videoViewerWF4.Text = "videoViewerWF4";
            // 
            // videoViewerWF1
            // 
            this.videoViewerWF1.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF1.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF1.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF1.FullScreenEnabled = true;
            this.videoViewerWF1.Location = new System.Drawing.Point(6, 19);
            this.videoViewerWF1.Name = "videoViewerWF1";
            this.videoViewerWF1.RotateAngle = 0;
            this.videoViewerWF1.Size = new System.Drawing.Size(238, 175);
            this.videoViewerWF1.TabIndex = 2;
            // 
            // videoViewerWF5
            // 
            this.videoViewerWF5.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF5.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF5.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF5.FullScreenEnabled = true;
            this.videoViewerWF5.Location = new System.Drawing.Point(6, 19);
            this.videoViewerWF5.Name = "videoViewerWF5";
            this.videoViewerWF5.RotateAngle = 0;
            this.videoViewerWF5.Size = new System.Drawing.Size(238, 175);
            this.videoViewerWF5.TabIndex = 1;
            // 
            // videoViewerWF6
            // 
            this.videoViewerWF6.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF6.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF6.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF6.FullScreenEnabled = true;
            this.videoViewerWF6.Location = new System.Drawing.Point(6, 19);
            this.videoViewerWF6.Name = "videoViewerWF6";
            this.videoViewerWF6.RotateAngle = 0;
            this.videoViewerWF6.Size = new System.Drawing.Size(238, 175);
            this.videoViewerWF6.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.videoViewerWF9);
            this.groupBox5.Location = new System.Drawing.Point(1188, 522);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox5.Size = new System.Drawing.Size(292, 292);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "دوربین 4";
            // 
            // videoViewerWF9
            // 
            this.videoViewerWF9.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF9.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF9.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF9.FullScreenEnabled = true;
            this.videoViewerWF9.Location = new System.Drawing.Point(7, 28);
            this.videoViewerWF9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.videoViewerWF9.Name = "videoViewerWF9";
            this.videoViewerWF9.RotateAngle = 0;
            this.videoViewerWF9.Size = new System.Drawing.Size(278, 256);
            this.videoViewerWF9.TabIndex = 0;
            this.videoViewerWF9.Text = "videoViewerWF9";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 136);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(849, 193);
            this.dataGridView1.TabIndex = 25;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.txt_Source);
            this.groupBox6.Controls.Add(this.txt_Destination);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.txt_Receiver);
            this.groupBox6.Controls.Add(this.txt_Sender);
            this.groupBox6.Location = new System.Drawing.Point(10, 352);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox6.Size = new System.Drawing.Size(853, 124);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "اطلاعات حواله";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(360, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 20);
            this.label7.TabIndex = 40;
            this.label7.Text = "مبدا";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(353, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 20);
            this.label6.TabIndex = 40;
            this.label6.Text = "مقصد";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(775, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 40;
            this.label5.Text = "فروشنده";
            // 
            // txt_Source
            // 
            this.txt_Source.Location = new System.Drawing.Point(12, 34);
            this.txt_Source.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Source.Name = "txt_Source";
            this.txt_Source.ReadOnly = true;
            this.txt_Source.Size = new System.Drawing.Size(328, 27);
            this.txt_Source.TabIndex = 41;
            // 
            // txt_Destination
            // 
            this.txt_Destination.Location = new System.Drawing.Point(12, 69);
            this.txt_Destination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Destination.Name = "txt_Destination";
            this.txt_Destination.ReadOnly = true;
            this.txt_Destination.Size = new System.Drawing.Size(328, 27);
            this.txt_Destination.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(775, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.TabIndex = 40;
            this.label4.Text = "فرستنده";
            // 
            // txt_Receiver
            // 
            this.txt_Receiver.Location = new System.Drawing.Point(435, 69);
            this.txt_Receiver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Receiver.Name = "txt_Receiver";
            this.txt_Receiver.ReadOnly = true;
            this.txt_Receiver.Size = new System.Drawing.Size(328, 27);
            this.txt_Receiver.TabIndex = 41;
            // 
            // txt_Sender
            // 
            this.txt_Sender.Location = new System.Drawing.Point(435, 34);
            this.txt_Sender.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Sender.Name = "txt_Sender";
            this.txt_Sender.ReadOnly = true;
            this.txt_Sender.Size = new System.Drawing.Size(328, 27);
            this.txt_Sender.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(442, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 38;
            this.label1.Text = "عنوان حواله";
            // 
            // txt_ShipmentTitle
            // 
            this.txt_ShipmentTitle.Location = new System.Drawing.Point(10, 80);
            this.txt_ShipmentTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_ShipmentTitle.Name = "txt_ShipmentTitle";
            this.txt_ShipmentTitle.ReadOnly = true;
            this.txt_ShipmentTitle.Size = new System.Drawing.Size(420, 27);
            this.txt_ShipmentTitle.TabIndex = 39;
            // 
            // btn_Search
            // 
            this.btn_Search.Enabled = false;
            this.btn_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_Search.ForeColor = System.Drawing.Color.Black;
            this.btn_Search.Location = new System.Drawing.Point(10, 38);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(93, 34);
            this.btn_Search.TabIndex = 34;
            this.btn_Search.Text = "جستجو";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(439, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 36;
            this.label2.Text = "شماره حواله";
            // 
            // txt_Search
            // 
            this.txt_Search.Location = new System.Drawing.Point(114, 41);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(317, 27);
            this.txt_Search.TabIndex = 37;
            this.txt_Search.TextChanged += new System.EventHandler(this.txt_Search_TextChanged);
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button_FirstWeighing);
            this.groupBox7.Controls.Add(this.button_CaptureImage1);
            this.groupBox7.Location = new System.Drawing.Point(555, 18);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox7.Size = new System.Drawing.Size(308, 86);
            this.groupBox7.TabIndex = 27;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Weighing";
            // 
            // button_FirstWeighing
            // 
            this.button_FirstWeighing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_FirstWeighing.ForeColor = System.Drawing.Color.Black;
            this.button_FirstWeighing.Location = new System.Drawing.Point(7, 29);
            this.button_FirstWeighing.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_FirstWeighing.Name = "button_FirstWeighing";
            this.button_FirstWeighing.Size = new System.Drawing.Size(110, 35);
            this.button_FirstWeighing.TabIndex = 34;
            this.button_FirstWeighing.Text = "First Weighing";
            this.button_FirstWeighing.UseVisualStyleBackColor = true;
            this.button_FirstWeighing.Click += new System.EventHandler(this.button_FirstWeighing_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radioButton_Purchase);
            this.groupBox8.Controls.Add(this.radioButton_Sales);
            this.groupBox8.Location = new System.Drawing.Point(889, 18);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox8.Size = new System.Drawing.Size(131, 101);
            this.groupBox8.TabIndex = 28;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "تنظیمات سیستم";
            // 
            // radioButton_Purchase
            // 
            this.radioButton_Purchase.AutoSize = true;
            this.radioButton_Purchase.Location = new System.Drawing.Point(24, 61);
            this.radioButton_Purchase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_Purchase.Name = "radioButton_Purchase";
            this.radioButton_Purchase.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_Purchase.Size = new System.Drawing.Size(91, 24);
            this.radioButton_Purchase.TabIndex = 38;
            this.radioButton_Purchase.Text = "اطلاعات خرید";
            this.radioButton_Purchase.UseVisualStyleBackColor = true;
            // 
            // radioButton_Sales
            // 
            this.radioButton_Sales.AutoSize = true;
            this.radioButton_Sales.Checked = true;
            this.radioButton_Sales.Location = new System.Drawing.Point(19, 28);
            this.radioButton_Sales.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_Sales.Name = "radioButton_Sales";
            this.radioButton_Sales.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_Sales.Size = new System.Drawing.Size(96, 24);
            this.radioButton_Sales.TabIndex = 37;
            this.radioButton_Sales.TabStop = true;
            this.radioButton_Sales.Text = "اطلاعات فروش";
            this.radioButton_Sales.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.radioButton_80Ton);
            this.groupBox9.Controls.Add(this.radioButton_60Ton);
            this.groupBox9.Location = new System.Drawing.Point(1028, 18);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox9.Size = new System.Drawing.Size(114, 101);
            this.groupBox9.TabIndex = 39;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "تنظیمات باسکول";
            // 
            // radioButton_80Ton
            // 
            this.radioButton_80Ton.AutoSize = true;
            this.radioButton_80Ton.Location = new System.Drawing.Point(45, 61);
            this.radioButton_80Ton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_80Ton.Name = "radioButton_80Ton";
            this.radioButton_80Ton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_80Ton.Size = new System.Drawing.Size(57, 24);
            this.radioButton_80Ton.TabIndex = 38;
            this.radioButton_80Ton.Text = "80 تن";
            this.radioButton_80Ton.UseVisualStyleBackColor = true;
            // 
            // radioButton_60Ton
            // 
            this.radioButton_60Ton.AutoSize = true;
            this.radioButton_60Ton.Checked = true;
            this.radioButton_60Ton.Location = new System.Drawing.Point(45, 28);
            this.radioButton_60Ton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_60Ton.Name = "radioButton_60Ton";
            this.radioButton_60Ton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_60Ton.Size = new System.Drawing.Size(57, 24);
            this.radioButton_60Ton.TabIndex = 37;
            this.radioButton_60Ton.TabStop = true;
            this.radioButton_60Ton.Text = "60 تن";
            this.radioButton_60Ton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_Output);
            this.groupBox2.Controls.Add(this.radioButton_Input);
            this.groupBox2.Location = new System.Drawing.Point(1149, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(115, 101);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "تنظیمات ایستگاه";
            // 
            // radioButton_Output
            // 
            this.radioButton_Output.AutoSize = true;
            this.radioButton_Output.Location = new System.Drawing.Point(41, 61);
            this.radioButton_Output.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_Output.Name = "radioButton_Output";
            this.radioButton_Output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_Output.Size = new System.Drawing.Size(62, 24);
            this.radioButton_Output.TabIndex = 38;
            this.radioButton_Output.Text = "خروجی";
            this.radioButton_Output.UseVisualStyleBackColor = true;
            // 
            // radioButton_Input
            // 
            this.radioButton_Input.AutoSize = true;
            this.radioButton_Input.Checked = true;
            this.radioButton_Input.Location = new System.Drawing.Point(44, 28);
            this.radioButton_Input.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton_Input.Name = "radioButton_Input";
            this.radioButton_Input.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton_Input.Size = new System.Drawing.Size(58, 24);
            this.radioButton_Input.TabIndex = 37;
            this.radioButton_Input.TabStop = true;
            this.radioButton_Input.Text = "ورودی";
            this.radioButton_Input.UseVisualStyleBackColor = true;
            // 
            // videoViewerWF7
            // 
            this.videoViewerWF7.BackColor = System.Drawing.Color.Black;
            this.videoViewerWF7.FlipMode = Ozeki.Media.FlipMode.None;
            this.videoViewerWF7.FrameStretch = Ozeki.Media.FrameStretch.Uniform;
            this.videoViewerWF7.FullScreenEnabled = true;
            this.videoViewerWF7.Location = new System.Drawing.Point(6, 19);
            this.videoViewerWF7.Name = "videoViewerWF7";
            this.videoViewerWF7.RotateAngle = 0;
            this.videoViewerWF7.Size = new System.Drawing.Size(238, 175);
            this.videoViewerWF7.TabIndex = 0;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label12);
            this.groupBox10.Controls.Add(this.label14);
            this.groupBox10.Controls.Add(this.txt_NetWeight);
            this.groupBox10.Controls.Add(this.label15);
            this.groupBox10.Controls.Add(this.txt_FullWeight);
            this.groupBox10.Controls.Add(this.txt_EmptyWeight);
            this.groupBox10.Location = new System.Drawing.Point(10, 649);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox10.Size = new System.Drawing.Size(853, 124);
            this.groupBox10.TabIndex = 42;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "اطلاعات وزنی";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(349, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 20);
            this.label12.TabIndex = 40;
            this.label12.Text = "وزن خالص";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(787, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 20);
            this.label14.TabIndex = 40;
            this.label14.Text = "وزن پر";
            // 
            // txt_NetWeight
            // 
            this.txt_NetWeight.Location = new System.Drawing.Point(12, 34);
            this.txt_NetWeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_NetWeight.Name = "txt_NetWeight";
            this.txt_NetWeight.ReadOnly = true;
            this.txt_NetWeight.Size = new System.Drawing.Size(328, 27);
            this.txt_NetWeight.TabIndex = 41;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(770, 38);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 20);
            this.label15.TabIndex = 40;
            this.label15.Text = "وزن خالی";
            // 
            // txt_FullWeight
            // 
            this.txt_FullWeight.Location = new System.Drawing.Point(435, 69);
            this.txt_FullWeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_FullWeight.Name = "txt_FullWeight";
            this.txt_FullWeight.ReadOnly = true;
            this.txt_FullWeight.Size = new System.Drawing.Size(328, 27);
            this.txt_FullWeight.TabIndex = 41;
            // 
            // txt_EmptyWeight
            // 
            this.txt_EmptyWeight.Location = new System.Drawing.Point(435, 34);
            this.txt_EmptyWeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_EmptyWeight.Name = "txt_EmptyWeight";
            this.txt_EmptyWeight.ReadOnly = true;
            this.txt_EmptyWeight.Size = new System.Drawing.Size(328, 27);
            this.txt_EmptyWeight.TabIndex = 41;
            // 
            // txt_TransportCode
            // 
            this.txt_TransportCode.Location = new System.Drawing.Point(435, 34);
            this.txt_TransportCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_TransportCode.Name = "txt_TransportCode";
            this.txt_TransportCode.ReadOnly = true;
            this.txt_TransportCode.Size = new System.Drawing.Size(306, 27);
            this.txt_TransportCode.TabIndex = 41;
            // 
            // txt_Car
            // 
            this.txt_Car.Location = new System.Drawing.Point(435, 70);
            this.txt_Car.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Car.Name = "txt_Car";
            this.txt_Car.ReadOnly = true;
            this.txt_Car.Size = new System.Drawing.Size(306, 27);
            this.txt_Car.TabIndex = 41;
            // 
            // txt_Driver
            // 
            this.txt_Driver.Location = new System.Drawing.Point(435, 108);
            this.txt_Driver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Driver.Name = "txt_Driver";
            this.txt_Driver.ReadOnly = true;
            this.txt_Driver.Size = new System.Drawing.Size(306, 27);
            this.txt_Driver.TabIndex = 41;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(749, 37);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 20);
            this.label11.TabIndex = 40;
            this.label11.Text = "شماره بارنامه";
            // 
            // txt_DriverID
            // 
            this.txt_DriverID.Location = new System.Drawing.Point(12, 70);
            this.txt_DriverID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_DriverID.Name = "txt_DriverID";
            this.txt_DriverID.ReadOnly = true;
            this.txt_DriverID.Size = new System.Drawing.Size(258, 27);
            this.txt_DriverID.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(791, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 40;
            this.label3.Text = "راننده";
            // 
            // txt_LicenseCode
            // 
            this.txt_LicenseCode.Location = new System.Drawing.Point(12, 34);
            this.txt_LicenseCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_LicenseCode.Name = "txt_LicenseCode";
            this.txt_LicenseCode.ReadOnly = true;
            this.txt_LicenseCode.Size = new System.Drawing.Size(258, 27);
            this.txt_LicenseCode.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(789, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 20);
            this.label10.TabIndex = 40;
            this.label10.Text = "ماشین";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(278, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "کد شناسایی راننده";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(292, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 20);
            this.label8.TabIndex = 40;
            this.label8.Text = "شماره گواهینامه";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.txt_LicenseCode);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txt_DriverID);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txt_Driver);
            this.groupBox4.Controls.Add(this.txt_Car);
            this.groupBox4.Controls.Add(this.txt_TransportCode);
            this.groupBox4.Location = new System.Drawing.Point(10, 485);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox4.Size = new System.Drawing.Size(853, 155);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "اطلاعات بارنامه";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 741);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_ShipmentTitle);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.CameraBox3);
            this.Controls.Add(this.CameraBox2);
            this.Controls.Add(this.CameraBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("IRANSans", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PASCO Weighing & Dispatching";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.CameraBox1.ResumeLayout(false);
            this.CameraBox2.ResumeLayout(false);
            this.CameraBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox CameraBox1;
        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.GroupBox CameraBox2;
        private System.Windows.Forms.GroupBox CameraBox3;
        private System.Windows.Forms.Button button_CaptureImage1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Button button_Disconnect;
        //private Ozeki.Media.VideoViewerWF videoViewerWF2;
        //private Ozeki.Media.VideoViewerWF videoViewerWF3;
        //private Ozeki.Media.VideoViewerWF videoViewerWF4;
        //private Ozeki.Media.VideoViewerWF videoViewerWF1;
        //private Ozeki.Media.VideoViewerWF videoViewerWF5;
        //private Ozeki.Media.VideoViewerWF videoViewerWF6;
        private System.Windows.Forms.GroupBox groupBox5;
        //private Ozeki.Media.VideoViewerWF videoViewerWF9;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_ShipmentTitle;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button_FirstWeighing;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radioButton_Purchase;
        private System.Windows.Forms.RadioButton radioButton_Sales;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RadioButton radioButton_80Ton;
        private System.Windows.Forms.RadioButton radioButton_60Ton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_Output;
        private System.Windows.Forms.RadioButton radioButton_Input;
        //private Ozeki.Media.VideoViewerWF videoViewerWF7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Source;
        private System.Windows.Forms.TextBox txt_Destination;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Receiver;
        private System.Windows.Forms.TextBox txt_Sender;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_NetWeight;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt_FullWeight;
        private System.Windows.Forms.TextBox txt_EmptyWeight;
        private System.Windows.Forms.TextBox txt_TransportCode;
        private System.Windows.Forms.TextBox txt_Car;
        private System.Windows.Forms.TextBox txt_Driver;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_DriverID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_LicenseCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;

    }
}

