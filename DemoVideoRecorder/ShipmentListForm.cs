using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace AshaWeighing
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
        private DataTable _weighingOrderTable;
        public string shipmentCode;
        public ShipmentListForm()
        {
            InitializeComponent();
            Load += ShipmentListForm_Load;
            Initialize();
        }

        void ShipmentListForm_Load(object sender, EventArgs e)
        {
            Font = myFont;
            doSearch("");
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

                _dbCommand = new SqlCommand("SELECT Code as [کد توزین], Title as [عنوان], Field1 as [اطلاعات 1], Field2 as [اطلاعات 2], " +
                                            "Field3 as [اطلاعات 3], Field4 as [اطلاعات 4], Field5 as [اطلاعات 5], Field6 as [اطلاعات 6], " + 
                                            "Field7 as [اطلاعات 7], SOShipmentCode as [شماره حمل فروش], POShipmentCode as [شماره حمل خرید], " +
                                            "InvTransactionCode as [شماره تراکنش انبار], Reference as [عطف به] FROM WMLog_WeighingOrder  " +
                                            "WHERE FormStatusCode='Wgh_Weighing'");
                _dbCommand.Connection = _dbConnector;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                _weighingOrderTable = new DataTable();
                _dbAdapter.Fill(_weighingOrderTable);

                if (_weighingOrderTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = _weighingOrderTable;
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
            shipmentCode = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
