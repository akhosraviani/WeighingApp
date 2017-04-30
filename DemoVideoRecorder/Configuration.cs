using AshaWeighing.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AshaWeighing
{
    public partial class Configuration : Form
    {
        private SqlConnection _dbConnection;

        public Configuration()
        {
            InitializeComponent();
            this.FormClosing += Configuration_FormClosing;
            this.Load += Configuration_Load;
            _dbConnection = new SqlConnection(Globals.ConnectionString);
        }

        void Configuration_Load(object sender, EventArgs e)
        {
            var macAddr =
            (
                from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            if (_dbConnection.State != ConnectionState.Open)
            {
                try
                {
                    _dbConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SELECT Code, Title FROM SISys_SubSysConfigDetail WHERE Code='" + macAddr + "' ", _dbConnection);
                    
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    DataTable conf = new DataTable();
                    da.Fill(conf);
                    if(conf.Rows.Count > 0)
                    {
                        for (int i = 0; i < conf.Rows.Count; i++)
                        {
                            var Label = new Label();
                            Label.Name = conf.Rows[i].Field<string>("Code");
                            Label.Text = conf.Rows[i].Field<string>("Title");
                            tableLayoutPanel1.Controls.Add(Label);
                        }
                    }
                    else
                    {
                        //sqlCommand = new SqlCommand("INSERT INTO SISys_SubSysConfig(Code, Title, SubSystemCode, Version, FormStatusCode, CreatorCode, CreationDate) " +
                        //                            "VALUES (@Code, @Title, @SubSystemCode, @Version, @FormStatusCode, @CreatorCode, @CreationDate)", _dbConnection);

                        //// set up the parameters
                        //sqlCommand.Parameters.Add("@Code", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@SubSystemCode", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@Version", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@FormStatusCode", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                        //sqlCommand.Parameters.Add("@CreationDate", SqlDbType.DateTime);

                        //// set parameter values
                        //sqlCommand.Parameters["@Code"].Value = macAddr;
                        //sqlCommand.Parameters["@Title"].Value = "تنظیمات کامپیوتر " + macAddr;
                        //sqlCommand.Parameters["@SubSystemCode"].Value = "WMLog";
                        //sqlCommand.Parameters["@Version"].Value = "0";
                        //sqlCommand.Parameters["@FormStatusCode"].Value = "Cfg_Active";
                        //sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                        //sqlCommand.Parameters["@CreationDate"].Value = DateTime.Now;

                        //sqlCommand.ExecuteNonQuery();
                        //sqlCommand.Dispose();

                        sqlCommand = new SqlCommand("INSERT INTO SISys_SubSysConfigDetail(ConfigurationCode, Code, Title, Value, CreatorCode, CreationDate) " +
                                                    "VALUES (@ConfigurationCode, @Code, @Title, @Value, @CreatorCode, @CreationDate)", _dbConnection);

                        // set up the parameters
                        sqlCommand.Parameters.Add("@ConfigurationCode", SqlDbType.NVarChar, 64);
                        sqlCommand.Parameters.Add("@Code", SqlDbType.NVarChar, 64);
                        sqlCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 64);
                        sqlCommand.Parameters.Add("@Value", SqlDbType.NVarChar, 64);
                        sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                        sqlCommand.Parameters.Add("@CreationDate", SqlDbType.DateTime);


                        using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM SISys_SubSysConfig WHERE SubSystemCode='WMLog' AND FormStatusCode='Cfg_Active'"
                                                        , _dbConnection))
                        {
                            da = new SqlDataAdapter(cmd);
                            conf = new DataTable();
                            da.Fill(conf);
                            ToolStripMenuItem[] items = new ToolStripMenuItem[conf.Rows.Count];

                            for (int i = 0; i < conf.Rows.Count; i++)
                            {
                                // set parameter values
                                sqlCommand.Parameters["@ConfigurationCode"].Value = conf.Rows[i].Field<string>("Code");
                                sqlCommand.Parameters["@Code"].Value = macAddr;
                                sqlCommand.Parameters["@Title"].Value = "تنظیمات در کامپیوتر " + macAddr;
                                sqlCommand.Parameters["@Value"].Value = "Visible";
                                sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                                sqlCommand.Parameters["@CreationDate"].Value = DateTime.Now;

                                sqlCommand.ExecuteNonQuery();

                            }
                        }

                        sqlCommand.Dispose();
                    }
                    
                    _dbConnection.Close();
                }
                catch (Exception exp)
                {
                    LogHelper.Log(LogTarget.Database, "Error", null, "ConnectDatabase: " + exp.Message, Globals.UserCode);
                }
            }
        }

        void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.txtTitle1.Text != Settings.Default.Setting1 ||
            //    this.txtBascolPort1.Text != Settings.Default.BascolPort1 ||
            //    this.txtCameraIP14.Text != Settings.Default.CameraIP14 ||
            //    this.txtCameraIP13.Text != Settings.Default.CameraIP13 ||
            //    this.txtCameraIP12.Text != Settings.Default.CameraIP12 ||
            //    this.txtCameraIP11.Text != Settings.Default.CameraIP11 ||
            //    this.txtTitle2.Text != Settings.Default.Setting2 ||
            //    this.txtBascolPort2.Text != Settings.Default.BascolPort2 ||
            //    this.txtCameraIP24.Text != Settings.Default.CameraIP24 ||
            //    this.txtCameraIP23.Text != Settings.Default.CameraIP23 ||
            //    this.txtCameraIP22.Text != Settings.Default.CameraIP22 ||
            //    this.txtCameraIP21.Text != Settings.Default.CameraIP21 ||
            //    this.txtTitle3.Text != Settings.Default.Setting3 ||
            //    this.txtBascolPort3.Text != Settings.Default.BascolPort3 ||
            //    this.txtCameraIP34.Text != Settings.Default.CameraIP34 ||
            //    this.txtCameraIP33.Text != Settings.Default.CameraIP33 ||
            //    this.txtCameraIP32.Text != Settings.Default.CameraIP32 ||
            //    this.txtCameraIP31.Text != Settings.Default.CameraIP31 ||
            //    this.txtTitle4.Text != Settings.Default.Setting4 ||
            //    this.txtBascolPort4.Text != Settings.Default.BascolPort4 ||
            //    this.txtCameraIP44.Text != Settings.Default.CameraIP44 ||
            //    this.txtCameraIP43.Text != Settings.Default.CameraIP43 ||
            //    this.txtCameraIP42.Text != Settings.Default.CameraIP42 ||
            //    this.txtCameraIP41.Text != Settings.Default.CameraIP41)
            //{
            //    DialogResult result = MessageBox.Show("تنظیمات تغییر کرده است. آیا می خواهید اطلاعات را ذخیره کنید؟", "ذخیره اطلاعات",
            //                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
            //    if (result == System.Windows.Forms.DialogResult.Cancel)
            //    {
            //        // Return to application
            //        e.Cancel = true;
            //    }
            //    else if (result == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        Settings.Default.Setting1 = this.txtTitle1.Text;
            //        Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
            //        Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
            //        Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
            //        Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
            //        Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            //        Settings.Default.Setting2 = this.txtTitle2.Text;
            //        Settings.Default.CameraIP21 = this.txtCameraIP21.Text;
            //        Settings.Default.CameraIP22 = this.txtCameraIP22.Text;
            //        Settings.Default.CameraIP23 = this.txtCameraIP23.Text;
            //        Settings.Default.CameraIP24 = this.txtCameraIP24.Text;
            //        Settings.Default.BascolPort2 = this.txtBascolPort2.Text;

            //        Settings.Default.Setting3 = this.txtTitle3.Text;
            //        Settings.Default.CameraIP31 = this.txtCameraIP31.Text;
            //        Settings.Default.CameraIP32 = this.txtCameraIP32.Text;
            //        Settings.Default.CameraIP33 = this.txtCameraIP33.Text;
            //        Settings.Default.CameraIP34 = this.txtCameraIP34.Text;
            //        Settings.Default.BascolPort3 = this.txtBascolPort3.Text;

            //        Settings.Default.Setting4 = this.txtTitle4.Text;
            //        Settings.Default.CameraIP41 = this.txtCameraIP41.Text;
            //        Settings.Default.CameraIP42 = this.txtCameraIP42.Text;
            //        Settings.Default.CameraIP43 = this.txtCameraIP43.Text;
            //        Settings.Default.CameraIP44 = this.txtCameraIP44.Text;
            //        Settings.Default.BascolPort4 = this.txtBascolPort4.Text;
            //        // Save settings
            //        Settings.Default.Save();
            //    }
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Settings.Default.Setting1 = this.txtTitle1.Text;
            //Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
            //Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
            //Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
            //Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
            //Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            //Settings.Default.Setting2 = this.txtTitle2.Text;
            //Settings.Default.CameraIP21 = this.txtCameraIP21.Text;
            //Settings.Default.CameraIP22 = this.txtCameraIP22.Text;
            //Settings.Default.CameraIP23 = this.txtCameraIP23.Text;
            //Settings.Default.CameraIP24 = this.txtCameraIP24.Text;
            //Settings.Default.BascolPort2 = this.txtBascolPort2.Text;

            //Settings.Default.Setting3 = this.txtTitle3.Text;
            //Settings.Default.CameraIP31 = this.txtCameraIP31.Text;
            //Settings.Default.CameraIP32 = this.txtCameraIP32.Text;
            //Settings.Default.CameraIP33 = this.txtCameraIP33.Text;
            //Settings.Default.CameraIP34 = this.txtCameraIP34.Text;
            //Settings.Default.BascolPort3 = this.txtBascolPort3.Text;

            //Settings.Default.Setting4 = this.txtTitle4.Text;
            //Settings.Default.CameraIP41 = this.txtCameraIP41.Text;
            //Settings.Default.CameraIP42 = this.txtCameraIP42.Text;
            //Settings.Default.CameraIP43 = this.txtCameraIP43.Text;
            //Settings.Default.CameraIP44 = this.txtCameraIP44.Text;
            //Settings.Default.BascolPort4 = this.txtBascolPort4.Text;
            //// Save settings
            //Settings.Default.Save();

            //MessageBox.Show("تنظیمات جدید با موفقیت ذخیره شد.", "ذخیره اطلاعات",
            //    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveExit_Click(object sender, EventArgs e)
        {
            //Settings.Default.Setting1 = this.txtTitle1.Text;
            //Settings.Default.CameraIP11 = this.txtCameraIP11.Text;
            //Settings.Default.CameraIP12 = this.txtCameraIP12.Text;
            //Settings.Default.CameraIP13 = this.txtCameraIP13.Text;
            //Settings.Default.CameraIP14 = this.txtCameraIP14.Text;
            //Settings.Default.BascolPort1 = this.txtBascolPort1.Text;

            //Settings.Default.Setting2 = this.txtTitle2.Text;
            //Settings.Default.CameraIP21 = this.txtCameraIP21.Text;
            //Settings.Default.CameraIP22 = this.txtCameraIP22.Text;
            //Settings.Default.CameraIP23 = this.txtCameraIP23.Text;
            //Settings.Default.CameraIP24 = this.txtCameraIP24.Text;
            //Settings.Default.BascolPort2 = this.txtBascolPort2.Text;

            //Settings.Default.Setting3 = this.txtTitle3.Text;
            //Settings.Default.CameraIP31 = this.txtCameraIP31.Text;
            //Settings.Default.CameraIP32 = this.txtCameraIP32.Text;
            //Settings.Default.CameraIP33 = this.txtCameraIP33.Text;
            //Settings.Default.CameraIP34 = this.txtCameraIP34.Text;
            //Settings.Default.BascolPort3 = this.txtBascolPort3.Text;

            //Settings.Default.Setting4 = this.txtTitle4.Text;
            //Settings.Default.CameraIP41 = this.txtCameraIP41.Text;
            //Settings.Default.CameraIP42 = this.txtCameraIP42.Text;
            //Settings.Default.CameraIP43 = this.txtCameraIP43.Text;
            //Settings.Default.CameraIP44 = this.txtCameraIP44.Text;
            //Settings.Default.BascolPort4 = this.txtBascolPort4.Text;
            //// Save settings
            //Settings.Default.Save();
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
