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
                    using (SqlCommand cmd = new SqlCommand("Select Code, Title From WMLog_WeighingType"
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
                    using (SqlCommand cmd = new SqlCommand("Select DriverCode, Title From WMLog_Driver"
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
                }
            }
            catch (Exception exp)
            {
            }

            _dbConnection.Close();
        }
    }

}
