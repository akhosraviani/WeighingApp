using _03_Onvif_Network_Video_Recorder.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
            this.FormClosing += Configuration_FormClosing;
            this.Load += Configuration_Load;
        }

        void Configuration_Load(object sender, EventArgs e)
        {
            // Set Camera IP 1
            if (Settings.Default.CameraIP11 != null)
            {
                this.txtCameraIP1.Text = Settings.Default.CameraIP11;
            }

            // Set Camera IP 2
            if (Settings.Default.CameraIP12 != null)
            {
                this.txtCameraIP2.Text = Settings.Default.CameraIP12;
            }

            // Set Camera IP 3
            if (Settings.Default.CameraIP13 != null)
            {
                this.txtCameraIP3.Text = Settings.Default.CameraIP13;
            }

            // Set Camera IP 4
            if (Settings.Default.CameraIP14 != null)
            {
                this.txtCameraIP4.Text = Settings.Default.CameraIP14;
            }

            // Set bascol port
            if (Settings.Default.BascolPort1 != null)
            {
                this.txtBascolPort1.Text = Settings.Default.BascolPort1;
            }
        }

        void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.txtBascolPort1.Text != Settings.Default.BascolPort1 ||
                this.txtCameraIP4.Text != Settings.Default.CameraIP14 ||
                this.txtCameraIP3.Text != Settings.Default.CameraIP13 ||
                this.txtCameraIP2.Text != Settings.Default.CameraIP12 ||
                this.txtCameraIP1.Text != Settings.Default.CameraIP11)
            {
                DialogResult result = MessageBox.Show("تنظیمات تغییر کرده است. آیا می خواهید اطلاعات را ذخیره کنید؟", "ذخیره اطلاعات",
                                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    // Return to application
                    e.Cancel = true;
                }
                else if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Settings.Default.CameraIP11 = this.txtCameraIP1.Text;
                    Settings.Default.CameraIP12 = this.txtCameraIP2.Text;
                    Settings.Default.CameraIP13 = this.txtCameraIP3.Text;
                    Settings.Default.CameraIP14 = this.txtCameraIP4.Text;
                    Settings.Default.BascolPort1 = this.txtBascolPort1.Text;
                    
                    // Save settings
                    Settings.Default.Save();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.CameraIP11 = this.txtCameraIP1.Text;
            Settings.Default.CameraIP12 = this.txtCameraIP2.Text;
            Settings.Default.CameraIP13 = this.txtCameraIP3.Text;
            Settings.Default.CameraIP14 = this.txtCameraIP4.Text;
            Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            // Save settings
            Settings.Default.Save();

            MessageBox.Show("تنظیمات جدید با موفقیت ذخیره شد.", "ذخیره اطلاعات",
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveExit_Click(object sender, EventArgs e)
        {
            Settings.Default.CameraIP11 = this.txtCameraIP1.Text;
            Settings.Default.CameraIP12 = this.txtCameraIP2.Text;
            Settings.Default.CameraIP13 = this.txtCameraIP3.Text;
            Settings.Default.CameraIP14 = this.txtCameraIP4.Text;
            Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            // Save settings
            Settings.Default.Save();
            this.Close();
        }
    }
}
