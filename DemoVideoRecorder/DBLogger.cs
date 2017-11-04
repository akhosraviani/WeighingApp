using System;
using System.Data;
using System.Data.SqlClient;

namespace AshaWeighing
{
    public class DBLogger : LogBase

    {
        public override void Log(string type, string weighingOrderCode, string message, string userID)

        {

            lock (lockObj)

            {
                var dbConnection = new SqlConnection(Globals.ConnectionString);
                if (dbConnection.State != ConnectionState.Open)
                {
                    try
                    {
                        dbConnection.Open();
                        if (weighingOrderCode != null)
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO WMLog_WeighingLog(WeighingOrderCode, Action, UserID, ActionType, ActionDateTime) " +
                                                                   "VALUES('" + weighingOrderCode + "', '" + message.Replace("'", "") + "', '" + userID + "', '" + type + "', GETDATE())"
                                            , dbConnection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO WMLog_WeighingLog(Action, UserID, ActionType, ActionDateTime) " +
                                                                   "VALUES('" + message.Replace("'", "") + "', '" + userID + "', '" + type + "', GETDATE())"
                                            , dbConnection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        dbConnection.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
