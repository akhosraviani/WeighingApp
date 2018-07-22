using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
                    using (SqlCommand cmd = new SqlCommand("Select Code, Title From wminv_TransactionType Where Code In ('ProjectCons','ConsReturn','ManualIssue','ManualReceive','WasteReceive','WasteRecycle','WasteReturned','WasteSales','Contractor','ContractorRet','SalesOrder','PurchaseOrder','Import','Export','WIPReceive','WIPReturn','WIPIssue')"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbInventoryTransactionType.ValueMember = "Code";
                        cmbInventoryTransactionType.DisplayMember = "Title";
                        cmbInventoryTransactionType.DataSource = dt;
                        cmbInventoryTransactionType.SelectedValue = "WasteReceive";

                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbInventoryTransactionType.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                                          // autocomplete source to the combobox
                        cmbInventoryTransactionType.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 DriverCode, Title From WMLog_Driver Where BasculeProperty=1"
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
                        cmbDriver.SelectedIndex = -1;
                        cmbDriver.Text = "انتخاب...";

                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbDriver.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                        // autocomplete source to the combobox
                        cmbDriver.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMLog_Vehicle Where BasculeProperty=1"
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
                        cmbCarNo.SelectedIndex = -1;
                        cmbCarNo.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbCarNo.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                       // autocomplete source to the combobox
                        cmbCarNo.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMInv_Part Where BasculeProperty=1"
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
                        cmbPart.SelectedIndex = -1;
                        cmbPart.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbPart.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                      // autocomplete source to the combobox
                        cmbPart.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("select Code, Title from WMInv_Inventory where InventoryTypeCode = '100'"
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
                        cmbFromInventory.SelectedIndex = -1;
                        cmbFromInventory.Text = "انتخاب...";
                        cmbToInventory.ValueMember = "Code";
                        cmbToInventory.DisplayMember = "Title";
                        cmbToInventory.DataSource = dt;
                        cmbToInventory.SelectedIndex = -1;
                        cmbToInventory.Text = "انتخاب...";

                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbFromInventory.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                               // autocomplete source to the combobox
                        cmbFromInventory.AutoCompleteCustomSource = ac;
                        cmbToInventory.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                             // autocomplete source to the combobox
                        cmbToInventory.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WMInv_InventoryLotStatus"
                                        , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbFromLotCode.ValueMember = "Code";
                        cmbFromLotCode.DisplayMember = "Title";
                        cmbFromLotCode.DataSource = dt;
                        cmbFromLotCode.SelectedIndex = -1;
                        cmbFromLotCode.Text = "انتخاب...";
                        cmbToLotCode.ValueMember = "Code";
                        cmbToLotCode.DisplayMember = "Title";
                        cmbToLotCode.DataSource = dt;
                        cmbToLotCode.SelectedIndex = -1;
                        cmbToLotCode.Text = "انتخاب...";

                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbFromLotCode.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                             // autocomplete source to the combobox
                        cmbFromLotCode.AutoCompleteCustomSource = ac;
                        cmbToLotCode.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                           // autocomplete source to the combobox
                        cmbToLotCode.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Code + '|' + Title AS Title From FiCC_CostCenter"
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
                        cmbCostCenter.SelectedIndex = -1;
                        cmbCostCenter.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbCostCenter.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                            // autocomplete source to the combobox
                        cmbCostCenter.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Code + '|' + Title AS Title From FiGL_CostAccount"
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
                        cmbAccountCode.SelectedIndex = -1;
                        cmbAccountCode.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbAccountCode.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                             // autocomplete source to the combobox
                        cmbAccountCode.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WFFC_Contact Where BasculeProperty=1"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbSender.ValueMember = "Code";
                        cmbSender.DisplayMember = "Title";
                        cmbSender.DataSource = dt;
                        cmbSender.SelectedIndex = -1;
                        cmbSender.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbSender.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                        // autocomplete source to the combobox
                        cmbSender.AutoCompleteCustomSource = ac;
                    }
                    using (SqlCommand cmd = new SqlCommand("Select Top 100 Code, Title From WFFC_Contact Where BasculeProperty=1"
                                                            , _dbConnection))
                    {
                        SqlDataReader reader;

                        reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Code", typeof(string));
                        dt.Columns.Add("Title", typeof(string));
                        dt.Load(reader);

                        cmbPeymankar.ValueMember = "Code";
                        cmbPeymankar.DisplayMember = "Title";
                        cmbPeymankar.DataSource = dt;
                        cmbPeymankar.SelectedIndex = -1;
                        cmbPeymankar.Text = "انتخاب...";
                        AutoCompleteStringCollection ac = new AutoCompleteStringCollection();
                        foreach (DataRow row in dt.Rows)
                        {
                            //looping through the datatable and adding values from datatable into autocomplete collection
                            ac.Add(row["Title"].ToString());
                        }
                        cmbPeymankar.AutoCompleteSource = AutoCompleteSource.CustomSource; //assigning   
                                                                                           // autocomplete source to the combobox
                        cmbPeymankar.AutoCompleteCustomSource = ac;
                    }
                }

                DateTime d = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                txtWeighingDate.Text = string.Format("{0}/{1}/{2} {3}:{4}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), pc.GetHour(d), pc.GetMinute(d));
            }
            catch (Exception exp)
            {
            }
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
                try
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
                    sqlCommand.Parameters.Add("@FromLotCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ToLotCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@FromLocationCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ToLocationCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CostCenterCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CostAccountCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ContactCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@DriverCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@VehicleCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@FromSource", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ToDestination", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@Note", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@PersonnelCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@PositionCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ComponentCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@CreatorCode", SqlDbType.NVarChar, 64);
                    sqlCommand.Parameters.Add("@ReturnMessage", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // set parameter values
                    sqlCommand.Parameters["@GroupCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@OrderTypeCode"].Value = cmbWeighingTypes.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@WeighingOrderCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@TransactionTypeCode"].Value = cmbInventoryTransactionType.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@TransactionDateTime"].Value = DateTime.Now;
                    sqlCommand.Parameters["@PartCode"].Value = cmbPart.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@ReceivedQuantity"].Value = 0;
                    sqlCommand.Parameters["@IssuedQuantity"].Value = 0;
                    sqlCommand.Parameters["@UnitOfMeasureCode"].Value = "Kg";
                    sqlCommand.Parameters["@FromInventoryCode"].Value = cmbFromInventory.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@FromLotCode"].Value = cmbFromLotCode.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@ToInventoryCode"].Value = cmbToInventory.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@ToLotCode"].Value = cmbToLotCode.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@CostCenterCode"].Value = cmbCostCenter.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@FromLocationCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@ToLocationCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@CostAccountCode"].Value = cmbAccountCode.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@ContactCode"].Value = cmbSender.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@DriverCode"].Value = cmbDriver.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@VehicleCode"].Value = cmbCarNo.SelectedValue ?? DBNull.Value;
                    sqlCommand.Parameters["@FromSource"].Value = DBNull.Value;
                    sqlCommand.Parameters["@ToDestination"].Value = DBNull.Value;
                    sqlCommand.Parameters["@Note"].Value = txtNote.Text;
                    sqlCommand.Parameters["@PersonnelCode"].Value = Globals.PersonnelCode;
                    sqlCommand.Parameters["@PositionCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@ComponentCode"].Value = DBNull.Value;
                    sqlCommand.Parameters["@CreatorCode"].Value = Globals.UserCode;
                    sqlCommand.Parameters["@ReturnMessage"].Value = "";
                    sqlCommand.Parameters["@ReturnValue"].Value = 1;

                    sqlCommand.ExecuteNonQuery();
                    string returnMessage = Convert.ToString(sqlCommand.Parameters["@ReturnMessage"].Value);
                    int returnValue = Convert.ToInt32(sqlCommand.Parameters["@ReturnValue"].Value);

                    //LogHelper.Log(LogTarget.Database, "Database", _weighingOrderCode, "btnSaveData_Click: Weighing data saved. " +
                    //            "WeighingOrderCode=" + _weighingOrderCode +
                    //            ", CreatorCode=" + Globals.UserName +
                    //            ", SavedWeight=" + sevenSegmentWeight.Value +
                    //            ", IndicatorWeight=" + _indicatorWeight, Globals.UserCode);

                    if (returnValue == 1)
                    {
                        MessageBox.Show(returnMessage, "پیغام", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        sqlCommand.Dispose();
                    }
                    else if (returnValue != 1)
                    {
                        MessageBox.Show(returnMessage, "اخطار", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        sqlCommand.Dispose();
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, "اخطار", MessageBoxButtons.OK,
                           MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    sqlCommand.Dispose();
                }
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://172.20.1.30/ReportServer/Pages/ReportViewer.aspx?/AshaMES_PASCO_V03/WMLog_WeighingOrderInfo");
            Process.Start(sInfo);
        }
    }

}
