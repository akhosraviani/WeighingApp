namespace _03_Onvif_Network_Video_Recorder
{
    partial class App
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItem_file = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_config = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_check = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_wheighing = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_aboutUs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MenuItem_ShipmentsList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Font = new System.Drawing.Font("IRANSans", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_file,
            this.MenuItem_tools,
            this.MenuItem_aboutUs});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(1101, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItem_file
            // 
            this.MenuItem_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_config,
            this.MenuItem_exit});
            this.MenuItem_file.Name = "MenuItem_file";
            this.MenuItem_file.Size = new System.Drawing.Size(80, 24);
            this.MenuItem_file.Text = "اطلاعات پایه";
            // 
            // MenuItem_config
            // 
            this.MenuItem_config.Name = "MenuItem_config";
            this.MenuItem_config.Size = new System.Drawing.Size(117, 24);
            this.MenuItem_config.Text = "تنظیمات";
            this.MenuItem_config.Click += new System.EventHandler(this.MenuItem_config_Click);
            // 
            // MenuItem_exit
            // 
            this.MenuItem_exit.Name = "MenuItem_exit";
            this.MenuItem_exit.Size = new System.Drawing.Size(117, 24);
            this.MenuItem_exit.Text = "خروج";
            this.MenuItem_exit.Click += new System.EventHandler(this.MenuItem_exit_Click);
            // 
            // MenuItem_tools
            // 
            this.MenuItem_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_check,
            this.MenuItem_ShipmentsList,
            this.MenuItem_wheighing});
            this.MenuItem_tools.Name = "MenuItem_tools";
            this.MenuItem_tools.Size = new System.Drawing.Size(50, 24);
            this.MenuItem_tools.Text = "ابزارها";
            // 
            // MenuItem_check
            // 
            this.MenuItem_check.Name = "MenuItem_check";
            this.MenuItem_check.Size = new System.Drawing.Size(159, 24);
            this.MenuItem_check.Text = "بررسی دستگاه ها";
            this.MenuItem_check.Click += new System.EventHandler(this.MenuItem_check_Click);
            // 
            // MenuItem_wheighing
            // 
            this.MenuItem_wheighing.Name = "MenuItem_wheighing";
            this.MenuItem_wheighing.Size = new System.Drawing.Size(159, 24);
            this.MenuItem_wheighing.Text = "توزین";
            this.MenuItem_wheighing.Click += new System.EventHandler(this.MenuItem_wheighing_Click);
            // 
            // MenuItem_aboutUs
            // 
            this.MenuItem_aboutUs.Name = "MenuItem_aboutUs";
            this.MenuItem_aboutUs.Size = new System.Drawing.Size(59, 24);
            this.MenuItem_aboutUs.Text = "درباره ما";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Font = new System.Drawing.Font("IRANSans", 8.5F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(16, 0, 1, 0);
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(1101, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MenuItem_ShipmentsList
            // 
            this.MenuItem_ShipmentsList.Name = "MenuItem_ShipmentsList";
            this.MenuItem_ShipmentsList.Size = new System.Drawing.Size(159, 24);
            this.MenuItem_ShipmentsList.Text = "لیست محموله ها";
            this.MenuItem_ShipmentsList.Click += new System.EventHandler(this.MenuItem_ShipmentsList_Click);
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1101, 453);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("IRANSans", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "App";
            this.Text = "نرم افزار توزین پاسارگاد";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_file;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_config;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_exit;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_tools;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_check;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_wheighing;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_aboutUs;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_ShipmentsList;
    }
}