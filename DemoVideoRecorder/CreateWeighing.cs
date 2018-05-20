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
        private SqlCommand sqlCommand;

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
                sqlCommand = new SqlCommand("WMInv_000_InventoryWeighingOrder", _dbConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // set up the parameters
                sqlCommand.Parameters.Add("@GroupCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@OrderTypeCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@WeighingOrderCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@TransactionTypeCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@TransactionDateTime", SqlDbType.DateTime);
                sqlCommand.Parameters.Add("@PartCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ReceivedQuantity", SqlDbType.Decimal);
                sqlCommand.Parameters.Add("@IssuedQuantity", SqlDbType.Decimal);
                sqlCommand.Parameters.Add("@UnitOfMeasureCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@FromInventoryCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ToInventoryCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@FromLocationCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ToLocationCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@FromLotCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ToLotCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@CostCenterCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@CostAccountCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ContactCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@DriverCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@VehicleCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@Note", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@PersonnelCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ComponentCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                // set parameter values
                sqlCommand.Parameters["@OrderTypeCode"].Value = cmbWeighingTypes.SelectedValue;
                sqlCommand.Parameters["@WeighingTypeCode"].Value = cmbWeighingTypes.SelectedValue;
                sqlCommand.Parameters["@OperationCode"].Value = cmbOperationCode.SelectedValue;
                sqlCommand.Parameters["@Weight"].Value = int.Parse(sevenSegmentWeight.Value);
                sqlCommand.Parameters["@Image1"].Value = (object)imageToByteArray(imgCamera1.Image) ?? DBNull.Value;
                sqlCommand.Parameters["@Image2"].Value = (object)imageToByteArray(imgCamera2.Image) ?? DBNull.Value;
                sqlCommand.Parameters["@Image3"].Value = (object)imageToByteArray(imgCamera3.Image) ?? DBNull.Value;
                sqlCommand.Parameters["@Image4"].Value = (object)imageToByteArray(imgCamera4.Image) ?? DBNull.Value;
                sqlCommand.Parameters["@MachineCode"].Value = Globals.WeighingMachineCode;
                sqlCommand.Parameters["@ResponsibleCode"].Value = Globals.PersonnelCode;
                sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                sqlCommand.Parameters["@ReturnMessage"].Value = "";
                sqlCommand.Parameters["@ReturnValue"].Value = 1;

                sqlCommand.ExecuteNonQuery();
                string returnMessage = Convert.ToString(sqlCommand.Parameters["@ReturnMessage"].Value);
                int returnValue = Convert.ToInt32(sqlCommand.Parameters["@ReturnValue"].Value);

                LogHelper.Log(LogTarget.Database, "Database", _weighingOrderCode, "btnSaveData_Click: Weighing data saved. " +
                            "WeighingOrderCode=" + _weighingOrderCode +
                            ", CreatorCode=" + Globals.UserName +
                            ", SavedWeight=" + sevenSegmentWeight.Value +
                            ", IndicatorWeight=" + _indicatorWeight, Globals.UserCode);

                if (returnValue == 1)
                {
                    MessageBox.Show(returnMessage, "پیغام", MessageBoxButtons.OK,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    sqlCommand.Dispose();
                }
                else if (returnValue == 0)
                {
                    MessageBox.Show(returnMessage, "اخطار", MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    sqlCommand.Dispose();
                }
            }
        }
    }

}
