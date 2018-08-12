using System.Net;

namespace Awful.Parser.Models.Web
{
    public class Result
    {
        public Result(bool isSuccess = false, string html = "", string json = "", string type = "", string uri = "")
        {
            IsSuccess = isSuccess;
            ResultHtml = html;
            ResultJson = json;
            Type = type;
            AbsoluteUri = uri;
        }

        /// <summary>
        /// If the request we've recieved was gotten successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The result of the request, in HTML form.
        /// </summary>
        public string ResultHtml { get; set; }

        /// <summary>
        /// The Uri of the request
        /// </summary>
        public string AbsoluteUri { get; set; }

        /// <summary>
        /// The request Uri
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// The result of the request, in JSON form.
        /// </summary>
        public string ResultJson { get; set; }

        /// <summary>
        /// The type of the object.
        /// </summary>
        public string Type { get; set; }
    }

    public class AuthResult
    {
        public AuthResult(CookieContainer container, bool isSuccess = false, string error = "")
        {
            IsSuccess = isSuccess;
            AuthenticationCookieContainer = container;
            Error = error;
        }

        /// <summary>
        /// If the request we've recieved was gotten successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// If errored on authentication, will contain the error message.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The serialized authentication cookie from logging in.
        /// </summary>
        public CookieContainer AuthenticationCookieContainer { get; set; }
    }

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
