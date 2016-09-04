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
            this.txtTitle1.Text = Settings.Default.Setting1;
            this.txtCameraIP11.Text = Settings.Default.CameraIP11;
            this.txtCameraIP12.Text = Settings.Default.CameraIP12;
            this.txtCameraIP13.Text = Settings.Default.CameraIP13;
            this.txtCameraIP14.Text = Settings.Default.CameraIP14;
            this.txtBascolPort1.Text = Settings.Default.BascolPort1;

            this.txtTitle2.Text = Settings.Default.Setting2;
            this.txtCameraIP21.Text = Settings.Default.CameraIP21;
            this.txtCameraIP22.Text = Settings.Default.CameraIP22;
            this.txtCameraIP23.Text = Settings.Default.CameraIP23;
            this.txtCameraIP24.Text = Settings.Default.CameraIP24;
            this.txtBascolPort2.Text = Settings.Default.BascolPort2;

            this.txtTitle3.Text = Settings.Default.Setting3;
            this.txtCameraIP31.Text = Settings.Default.CameraIP31;
            this.txtCameraIP32.Text = Settings.Default.CameraIP32;
            this.txtCameraIP33.Text = Settings.Default.CameraIP33;
            this.txtCameraIP34.Text = Settings.Default.CameraIP34;
            this.txtBascolPort3.Text = Settings.Default.BascolPort3;

            this.txtTitle4.Text = Settings.Default.Setting4;
            this.txtCameraIP41.Text = Settings.Default.CameraIP41;
            this.txtCameraIP42.Text = Settings.Default.CameraIP42;
            this.txtCameraIP43.Text = Settings.Default.CameraIP43;
            this.txtCameraIP44.Text = Settings.Default.CameraIP44;
            this.txtBascolPort4.Text = Settings.Default.BascolPort4;
        }

        void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.txtBascolPort1.Text != Settings.Default.BascolPort1 ||
                this.txtCameraIP14.Text != Settings.Default.CameraIP14 ||
                this.txtCameraIP13.Text != Settings.Default.CameraIP13 ||
                this.txtCameraIP12.Text != Settings.Default.CameraIP12 ||
                this.txtCameraIP11.Text != Settings.Default.CameraIP11)
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
                    Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
                    Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
                    Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
                    Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
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
            Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
            Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
            Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
            Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
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
            Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
            Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
            Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
            Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
            Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            // Save settings
            Settings.Default.Save();
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
