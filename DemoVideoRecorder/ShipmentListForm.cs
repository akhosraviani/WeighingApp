using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class ShipmentListForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        private Font myFont, myFontBig;

        private SqlConnection _dbConnector;
        private SqlCommand _dbCommand;
        private SqlDataAdapter _dbAdapter;
        private DataTable _shipmentTable;
        public string shipmentCode;
        public ShipmentListForm()
        {
            InitializeComponent();
            this.Load += ShipmentListForm_Load;
            Initialize();
        }

        void ShipmentListForm_Load(object sender, EventArgs e)
        {
            this.Font = myFont;
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
            myFontBig = new Font(fonts.Families[0], 14F);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            doSearch(txtSearch.Text);
        }

        private void doSearch(string p)
        {
            try
            {
                var connection =
                    System.Configuration.ConfigurationManager.ConnectionStrings["AshaDbContext"].ConnectionString;
                if (_dbConnector == null)
                    _dbConnector = new SqlConnection(connection);
                if (_dbConnector.State != ConnectionState.Open)
                {
                    _dbConnector.Open();
                }

                _dbCommand = new SqlCommand("SELECT SDSO_Shipment.Code AS [کد محموله], SDSO_Shipment.Title AS [عنوان محموله], SDSO_Customer.Title AS [گیرنده], WMLog_Vehicle.CarrierNumber AS [شماره ماشین], WMLog_Driver.Title AS [نام راننده], WMLog_Driver.LicenseNumber AS [شماره گواهینامه], SDSO_Shipment.Guid " +
                                                "FROM SDSO_Shipment LEFT OUTER JOIN WMLog_Driver " +
                                                "ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WMLog_Vehicle " +
                                                "ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN SDSO_Customer " +
                                                "ON SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode WHERE (SDSO_Shipment.FormStatusCode LIKE '%Weighing%' OR SDSO_Shipment.FormStatusCode LIKE '%Loading%' ) AND SDSO_Shipment.ReceptionDate > DATEADD(DAY, -1, GETDATE()) AND SDSO_Shipment.Code LIKE '%" + txtSearch.Text + "%'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                _shipmentTable = new DataTable();
                _dbAdapter.Fill(_shipmentTable);

                if (_shipmentTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = _shipmentTable;
                }

                dataGridView1.DefaultCellStyle.NullValue = "---";
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 != 0)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.AliceBlue;
                    }
                }
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                _dbConnector.Close();
            }
            catch(Exception)
            {
                MessageBox.Show("مشکل در اتصال به پایگاه داده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                doSearch(txtSearch.Text);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.shipmentCode = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
