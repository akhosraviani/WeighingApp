using System;
using System.Collections.Generic;
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

        private SqlConnection _dbConnection;
        private SqlCommand _dbCommand;
        private SqlDataAdapter _dbAdapter;
        private DataTable _weighingOrderTable;
        private List<WeighingOrderType> _weighingTypes;
        public string shipmentCode;
        private string _weighingSelectListCriteria;
        private string _orderType;

        public string WeighingSelectListCriteria
        {
            get { return _weighingSelectListCriteria; }
            set
            {
                _weighingSelectListCriteria = value;
                doSearch("");
            }
        }

        public string OrderType
        {
            get { return _orderType; }
            set
            {
                _orderType = value;
                cmbWeighingTypes.SelectedValue = _orderType;
            }
        }

        public ShipmentListForm()
        {
            InitializeComponent();
            Load += ShipmentListForm_Load;
            Initialize();
        }

        void ShipmentListForm_Load(object sender, EventArgs e)
        {
            GetWeighingTypes();
            if(_orderType != null)
                cmbWeighingTypes.SelectedValue = _orderType;
            Font = myFont;
            doSearch("");
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
        private void ConnectDatabase()
        {
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(Globals.ConnectionString);
            if (_dbConnection.State != ConnectionState.Open)
            {
                try
                {
                    _dbConnection.Open();
                }
                catch (Exception)
                {
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            doSearch(txtSearch.Text);
        }
        private void GetWeighingTypes()
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();
            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Code, Title FROM WMLog_WeighingType"
                                                            , _dbConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable wieghingTypeTable = new DataTable();
                        da.Fill(wieghingTypeTable);
                        _weighingTypes = new List<WeighingOrderType>();
                        for (int i = 0; i < wieghingTypeTable.Rows.Count; i++)
                        {
                            _weighingTypes.Add(new WeighingOrderType()
                            {
                                Name = wieghingTypeTable.Rows[i].Field<string>("Title"),
                                Value = wieghingTypeTable.Rows[i].Field<string>("Code")
                            });
                        }

                        cmbWeighingTypes.DataSource = _weighingTypes;
                        cmbWeighingTypes.DisplayMember = "Name";
                        cmbWeighingTypes.ValueMember = "Value";
                        cmbWeighingTypes.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void doSearch(string p)
        {
            try
            {
                if (_dbConnection == null)
                    _dbConnection = new SqlConnection(Globals.ConnectionString);
                if (_dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection.Open();
                }

                _dbCommand = new SqlCommand(WeighingSelectListCriteria.Replace("@Code", p));
                _dbCommand.Connection = _dbConnection;
                _dbAdapter = new SqlDataAdapter(_dbCommand);
                _weighingOrderTable = new DataTable();
                _dbAdapter.Fill(_weighingOrderTable);

                if (_weighingOrderTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = _weighingOrderTable;
                }
                else
                {
                    dataGridView1.DataSource = null;
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

                _dbConnection.Close();
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

        private void cmbWeighingTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();

            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT SelectListCriteria "
                            + "FROM WMLog_WeighingType WHERE Code='" + cmbWeighingTypes.SelectedValue + "'"
                                                            , _dbConnection))
                    {
                        DataTable weighingTypeDetailTable = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(weighingTypeDetailTable);
                        if (weighingTypeDetailTable.Rows.Count > 0)
                        {
                            WeighingSelectListCriteria = weighingTypeDetailTable.Rows[0].Field<string>("SelectListCriteria");
                            doSearch(txtSearch.Text);
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            shipmentCode = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
        public string SafeFarsiStr(string input)
        {
            return input.Replace("ی", "ی").Replace("ک", "ک");
        }
    }
}
