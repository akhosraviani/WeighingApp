using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _03_Onvif_Network_Video_Recorder.Properties;
using System.Threading;
using System.Drawing.Text;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class App : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;
        public App()
        {
            InitializeComponent();
            Initialize();
            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            this.Load += App_Load;
            this.FormClosing += App_FormClosing;
        }

        private void Initialize()
        {
            byte[] fontData = Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8.5F);
        }

        void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("آیا می خواهید از برنامه خارج شوید؟", "خروج", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.Cancel)
            {
                // Return to application
                e.Cancel = true;
            }
            else
            {
                // Copy window location to app settings
                Settings.Default.AppWindowLocation = this.Location;

                // Copy window state to app settings
                Settings.Default.AppWindowState = this.WindowState;

                // Copy window size to app settings
                Settings.Default.AppWindowSize = this.Size;

                // Save settings
                Settings.Default.Save();
            }
        }
        void App_Load(object sender, EventArgs e)
        {
            this.Font = myFont;
            menuStrip1.Font = myFont;
            // Set window state
            this.WindowState = Settings.Default.AppWindowState;

            // Set window location
            if (Settings.Default.AppWindowLocation != null)
            {
                this.Location = Settings.Default.AppWindowLocation;
            }

            // Set window size
            if (Settings.Default.AppWindowSize != null)
            {
                this.Size = Settings.Default.AppWindowSize;
            }
        }

        private void MenuItem_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuItem_wheighing_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "راه اندازی توزین...";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            this.IsMdiContainer = true;
            this.HScroll = false;
            this.VScroll = false;
            WeighingForm weighingForm = new WeighingForm();
            weighingForm.WindowState = FormWindowState.Maximized;
            weighingForm.MdiParent = this;
            weighingForm.Show();

            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.Default;
        }

        private void MenuItem_config_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "راه اندازی تنظیمات...";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            this.IsMdiContainer = true;
            this.HScroll = false;
            this.VScroll = false;
            Configuration configForm = new Configuration();
            configForm.MdiParent = this;
            configForm.Show();

            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.Default;
        }

        private void MenuItem_check_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "راه اندازی ارتباط با دستگاه ها...";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            this.IsMdiContainer = true;
            this.HScroll = false;
            this.VScroll = false;
            CheckConnections checkForm = new CheckConnections();
            checkForm.MdiParent = this;
            checkForm.Show();

            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.Default;
        }

        private void MenuItem_ShipmentsList_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "راه اندازی لیست محموله ها...";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            this.IsMdiContainer = true;
            this.HScroll = false;
            this.VScroll = false;
            ShipmentListForm shipmentListForm = new ShipmentListForm();
            shipmentListForm.MdiParent = this;
            shipmentListForm.Show();

            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.Default;
        }

        private void MenuItem_aboutUs_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }
    }
}
