using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using AshaWeighing.Properties;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace AshaWeighing
{
    public partial class App : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();
        private SqlConnection _dbConnection;
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
            byte[] fontData = AshaWeighing.Properties.Resources.IRANSans_FaNum_;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, AshaWeighing.Properties.Resources.IRANSans_FaNum_.Length);
            AddFontMemResourceEx(fontPtr, (uint)AshaWeighing.Properties.Resources.IRANSans_FaNum_.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 8F);
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
            checkSettings();
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

        private void checkSettings()
        {
            try
            {
                var macAddr =
                                (
                                    from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                                    where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                                    select nic.GetPhysicalAddress().ToString()
                                ).FirstOrDefault();

                if (_dbConnection == null)
                    _dbConnection = new SqlConnection(Globals.ConnectionString);
                if (_dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT a.Code, a.Title FROM SISys_SubSysConfig  as a JOIN SISys_SubSysConfigDetail as b " +
                                                   "ON a.Code = b.ConfigurationCode " +
                                                   "WHERE a.SubSystemCode='WMLog' AND a.FormStatusCode='Cfg_Active' and b.Value='Visible' and b.Code='" + macAddr + "'"
                                                    , _dbConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable conf = new DataTable();
                        da.Fill(conf);

                        if (conf.Rows.Count <= 0)
                        {
                            if (MessageBox.Show("تنظیمات کامپیوتر شما در سیستم ثبت نشده است. آیا می‌خواهید تنظیمات پیش فرض برای شما ثبت شود؟", "راه اندازی تنظیمات",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                AddSettingForm addSetting = new AddSettingForm();
                                addSetting.Text = "تنظیمات کامپیوتر " + macAddr;

                                if (addSetting.ShowDialog() == DialogResult.OK)
                                {
                                    string input = addSetting.Text;
                                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO SISys_SubSysConfigDetail(ConfigurationCode, Code, Title, Value, CreatorCode, CreationDate) " +
                                                            "VALUES (@ConfigurationCode, @Code, @Title, @Value, @CreatorCode, @CreationDate)", _dbConnection))
                                    {
                                        // set up the parameters
                                        sqlCommand.Parameters.Add("@ConfigurationCode", SqlDbType.NVarChar, 64);
                                        sqlCommand.Parameters.Add("@Code", SqlDbType.NVarChar, 64);
                                        sqlCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 64);
                                        sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 64);
                                        sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                                        sqlCommand.Parameters.Add("@CreationDate", SqlDbType.DateTime);


                                        using (SqlCommand cmd2 = new SqlCommand("SELECT Code, Title FROM SISys_SubSysConfig WHERE SubSystemCode='WMLog' AND FormStatusCode='Cfg_Active'"
                                                                        , _dbConnection))
                                        {
                                            da = new SqlDataAdapter(cmd2);
                                            conf = new DataTable();
                                            da.Fill(conf);
                                            ToolStripMenuItem[] items = new ToolStripMenuItem[conf.Rows.Count];

                                            for (int i = 0; i < conf.Rows.Count; i++)
                                            {
                                                // set parameter values
                                                sqlCommand.Parameters["@ConfigurationCode"].Value = conf.Rows[i].Field<string>("Code");
                                                sqlCommand.Parameters["@Code"].Value = macAddr;
                                                sqlCommand.Parameters["@Title"].Value = input;
                                                sqlCommand.Parameters["@Value"].Value = "Visible";
                                                sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                                                sqlCommand.Parameters["@CreationDate"].Value = DateTime.Now;

                                                sqlCommand.ExecuteNonQuery();

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("لطفاً جهت ثبت تنظیمات با مدیر سیستم تماس بگیرید.", "راه اندازی تنظیمات", MessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show("لطفاً جهت ثبت تنظیمات با مدیر سیستم تماس بگیرید.", "راه اندازی تنظیمات", MessageBoxButtons.OK);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "راه اندازی تنظیمات", MessageBoxButtons.OK);
            }
            Globals.GetConfigurationDetails(Settings.Default.SelectedConfiguration);
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
            //weighingForm.WindowState = FormWindowState.Maximized;
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

        private void MenuItem_Config_Click_1(object sender, EventArgs e)
        {
            Configuration config = new Configuration();
            config.ShowDialog();
        }

        private void MenuItem_WeighingCreate_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "راه اندازی توزین جدید...";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.WaitCursor;

            this.IsMdiContainer = true;
            this.HScroll = false;
            this.VScroll = false;
            CreateWeighing CreateWeighingForm = new CreateWeighing();
            //weighingForm.WindowState = FormWindowState.Maximized;
            CreateWeighingForm.MdiParent = this;
            CreateWeighingForm.Show();

            toolStripStatusLabel1.Text = "";
            statusStrip1.Refresh();
            Cursor.Current = Cursors.Default;
        }
    }
}
