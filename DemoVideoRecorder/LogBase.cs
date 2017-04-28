namespace AshaWeighing
{
    public abstract class LogBase
    {
        protected readonly object lockObj = new object();

        public abstract void Log(string type, string weighingOrderCode, string message, string userID);
    }

    public enum LogTarget
    {
        File, Database, EventLog
    }
}
