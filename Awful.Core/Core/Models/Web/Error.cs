using System;
namespace Awful.Models.Web
{
    public class Error
    {
        public Error(string type = "", string reason = "", string stacktrace = "", bool isPaywall = false)
        {
            Reason = reason;
            StackTrace = stacktrace;
            Type = type;
            IsPaywall = isPaywall;
        }

        public string Type { get; set; }
        public string Reason { get; set; }
        public string StackTrace { get; set; }
        public bool IsPaywall { get; set; }
    }
}
