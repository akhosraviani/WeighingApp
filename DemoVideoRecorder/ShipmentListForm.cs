using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _03_Onvif_Network_Video_Recorder
{
    public partial class ShipmentListForm : Form
    {
        private SqlConnection _dbConnector;
        private SqlCommand _dbCommand;
        private SqlDataAdapter _dbAdapter;
        private DataTable _shipmentTable;
        public string shipmentCode;
        public ShipmentListForm()
        {
            InitializeComponent();
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

                _dbCommand = new SqlCommand("SELECT SDSO_Shipment.Code AS ShipmentCode, SDSO_Shipment.Title AS ShipmentTitle, SDSO_Shipment.TransportCode AS TransportCode, SDSO_Customer.Title AS Destination, WMLog_Vehicle.CarrierNumber, WMLog_Driver.Title AS DriverTitle, WMLog_Driver.LicenseNumber AS LicenseNumber, SDSO_Shipment.Guid " +
                                                "FROM SDSO_Shipment LEFT OUTER JOIN WMLog_Driver " +
                                                "ON SDSO_Shipment.DriverCode = WMLog_Driver.DriverCode LEFT OUTER JOIN WMLog_Vehicle " +
                                                "ON SDSO_Shipment.VehicleCode = WMLog_Vehicle.Code LEFT OUTER JOIN SDSO_Customer " +
                                                "ON SDSO_Shipment.CustomerCode = SDSO_Customer.CustomerCode WHERE SDSO_Shipment.FormStatusCode LIKE '%Weighing%' AND SDSO_Shipment.Code LIKE '%" + txtSearch.Text + "%'");
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
