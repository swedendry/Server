using System;

namespace Server.Extensions
{
    public static class ExceptionExtension
    {
        public static string ToMessage(this Exception ex)
        {
            var message = "[Server Error] ";
            while (ex != null)
            {
                message += ex.Message + " <<<< ";
                ex = ex.InnerException;
            }

            return message;
        }
    }
}
