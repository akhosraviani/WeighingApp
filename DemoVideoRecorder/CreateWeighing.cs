using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AshaWeighing
{
    public partial class CreateWeighing : Form
    {
        private SqlConnection _dbConnection;
        public CreateWeighing()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConnectDatabase()
        {
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(Globals.ConnectionString);
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
        }

        private void CreateWeighing_Load(object sender, EventArgs e)
        {
            if (_dbConnection == null || _dbConnection.State != ConnectionState.Open)
                ConnectDatabase();
            try
            {
                if (_dbConnection.State == ConnectionState.Open)
                {
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMLog_WeighingType"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbWeighingTypes.ValueMember = "Code";
                        cmbWeighingTypes.DisplayMember = "Title";
                        cmbWeighingTypes.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 DriverCode, Title From WMLog_Driver"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("DriverCode", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbDriver.ValueMember = "DriverCode";
                        cmbDriver.DisplayMember = "Title";
                        cmbDriver.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMLog_Vehicle"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbCarNo.ValueMember = "Code";
                        cmbCarNo.DisplayMember = "Title";
                        cmbCarNo.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMInv_Part"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbPart.ValueMember = "Code";
                        cmbPart.DisplayMember = "Title";
                        cmbPart.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMInv_Inventory"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbFromInventory.ValueMember = "Code";
                        cmbFromInventory.DisplayMember = "Title";
                        cmbFromInventory.DataSource = dt;
                        cmbToInventory.ValueMember = "Code";
                        cmbToInventory.DisplayMember = "Title";
                        cmbToInventory.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, LotCode From WMInv_InventoryLotStock"
                                        , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("LotCode", typeof(string));
                        dt.Load(reader);

                        cmbFromLotCode.ValueMember = "Code";
                        cmbFromLotCode.DisplayMember = "LotCode";
                        cmbFromLotCode.DataSource = dt;
                        cmbToLotCode.ValueMember = "Code";
                        cmbToLotCode.DisplayMember = "LotCode";
                        cmbToLotCode.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From FiCC_CostCenter"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbCostCenter.ValueMember = "Code";
                        cmbCostCenter.DisplayMember = "Title";
                        cmbCostCenter.DataSource = dt;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From FiGL_CostAccount"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbAccountCode.ValueMember = "Code";
                        cmbAccountCode.DisplayMember = "Title";
                        cmbAccountCode.DataSource = dt;
                    }
                }
            }
            catch (Exception exp)
            {
            }

            _dbConnection.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("میخواهید ثبت انجام شود؟"
                , "Confirm"
                , MessageBoxButtons.YesNo
                );
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                // I have to cancel button click event here

            }
        }
    }

}
