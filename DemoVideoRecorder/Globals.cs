using System;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;

namespace AshaWeighing
{
    public static class Globals
    {
        public static string UserCode = "";
        public static string UserName = "";
        public static string PersonnelCode = "";
        public static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AshaDbContext"].ConnectionString;
        public static string[] CameraAddress = new string[4];
        public static string[] CameraUsername = new string[4];
        public static string[] CameraPassword = new string[4];
        public static string WeighingMachineSerialPort = "";
        public static string WeighingMachineCode = "";
        public static string WeighingMachineTitle = "";
        public static int SerialPorBaudRate = 2400;
        public static int SerialPortDataBits = 8;
        public static Parity SerialPortParity = Parity.None;
        public static Handshake SerialPortHandshake = Handshake.None;
        public static StopBits SerialPortStopBits = StopBits.One;
        public static int Tolerance = 10;

        private static SqlConnection _dbConnection;
        
        public static void GetConfigurationDetails(string configCode)
        {
            if (_dbConnection == null)
                _dbConnection = new SqlConnection(ConnectionString);
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

            using (SqlCommand cmd = new SqlCommand("SELECT ConfigurationCode, Code, Title, Value FROM SISys_SubSysConfigDetail " +
                                                    "WHERE ConfigurationCode='" + configCode + "'"
                                                    , _dbConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable conf = new DataTable();
                da.Fill(conf);
                if (conf.Rows.Count > 0)
                {
                    CameraAddress[0] = Convert.ToString(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera1")["Value"]);
                    CameraAddress[1] = Convert.ToString(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera2")["Value"]);
                    CameraAddress[2] = Convert.ToString(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera3")["Value"]);
                    CameraAddress[3] = Convert.ToString(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera4")["Value"]);
                    CameraUsername[0] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera1Username")["Value"];
                    CameraUsername[1] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera2Username")["Value"];
                    CameraUsername[2] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera3Username")["Value"];
                    CameraUsername[3] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera4Username")["Value"];
                    CameraPassword[0] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera1Password")["Value"];
                    CameraPassword[1] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera2Password")["Value"];
                    CameraPassword[2] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera3Password")["Value"];
                    CameraPassword[3] = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Camera4Password")["Value"];
                    WeighingMachineSerialPort = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "BascolPort")["Value"];
                    WeighingMachineCode = (string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "MachineCode")["Value"];
                    SerialPorBaudRate = Convert.ToInt32(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "BaudRate")["Value"]);
                    SerialPortDataBits = Convert.ToInt32(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "DataBits")["Value"]);
                    SerialPortParity = ConvertToParity((string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Parity")["Value"]);
                    SerialPortHandshake = ConvertToHandshake((string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Handshake")["Value"]);
                    SerialPortStopBits = ConvertToStopBits((string)conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "StopBits")["Value"]);
                    Tolerance = Convert.ToInt32(conf.AsEnumerable().SingleOrDefault(r => r.Field<string>("Code") == "Tolerance")["Value"]);
                }
            }
        }

        private static Parity ConvertToParity(string value)
        {
            switch(value.ToLower())
            {
                case "none":
                    return Parity.None;

                case "odd":
                    return Parity.Odd;

                case "even":
                    return Parity.Even;

                case "mark":
                    return Parity.Mark;

                case "space":
                    return Parity.Space;

                default:
                    return Parity.None;
            }
        }

        private static Handshake ConvertToHandshake(string value)
        {
            switch(value.ToLower())
            {
                case "none":
                    return Handshake.None;

                case "requesttosend":
                    return Handshake.RequestToSend;

                case "requesttosendxonxoff":
                    return Handshake.RequestToSendXOnXOff;

                case "xonxoff":
                    return Handshake.XOnXOff;

                default:
                    return Handshake.None;
            }
        }

        private static StopBits ConvertToStopBits(string value)
        {
            switch(value.ToLower())
            {
                case "none":
                    return StopBits.None;

                case "one":
                    return StopBits.One;

                case "onepointfive":
                    return StopBits.OnePointFive;

                case "two":
                    return StopBits.Two;

                default:
                    return StopBits.None;
            }
        }
    }
}
