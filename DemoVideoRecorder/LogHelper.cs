namespace AshaWeighing
{
    public static class LogHelper

    {
        private static LogBase logger = null;

        public static void Log(LogTarget target, string type, string weighingOrderCode, string message, string userID)

        {
            switch (target)
            {
                case LogTarget.Database:
                    logger = new DBLogger();
                    logger.Log(type, weighingOrderCode, message, userID);
                    break;


                default:
                    return;

            }
        }
    }
}
