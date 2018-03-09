using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Awful.Models.Web;
using Newtonsoft.Json;

namespace Awful.Managers
{
    public class WebManager
    {
        public WebManager(CookieContainer authenticationCookie = null)
        {
            if (authenticationCookie != null) {
                CookieContainer = authenticationCookie;
            }
            else
            {
                CookieContainer = new CookieContainer();
            }
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                UseDefaultCredentials = false,
                CookieContainer = CookieContainer
            };
            Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true
            };
            Client.DefaultRequestHeaders.Add("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            Client.DefaultRequestHeaders.Add("Accept-Language", "ja,en-US;q=0.8,en;q=0.6");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, sdch");
            Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        }

        public HttpClient Client { get; }
        public CookieContainer CookieContainer { get; }

    const string Accept = "text/html, application/xhtml+xml, */*";

        const string PostContentType = "application/x-www-form-urlencoded";

        const string ReplyBoundary = "----WebKitFormBoundaryYRBJZZBPUZAdxj3S";
        const string EditBoundary = "----WebKitFormBoundaryksMFcMGBHc3jdB0P";
        const string ReplyContentType = "multipart/form-data; boundary=" + ReplyBoundary;
        const string EditContentType = "multipart/form-data; boundary=" + EditBoundary;

        const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2593.0 Safari/537.36";

        public async Task<Result> GetDataAsync(string uri) {
            string html = "";
            try
            {
                Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
                var result = await Client.GetAsync(new Uri(uri));
                var stream = await result.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    html = reader.ReadToEnd();
                    return new Result(result.IsSuccessStatusCode, html, "", "", result.RequestMessage.RequestUri.AbsoluteUri);
                }
            }
            catch (Exception ex)
            {
                var error = new Error("", ex.Message, ex.StackTrace, false);
                return new Result(false, html, JsonConvert.SerializeObject(error), "", uri);
            }
        }

        public async Task<Result> PostDataAsync(string uri, FormUrlEncodedContent data) {
            var html = "";
            try
            {
                Client.DefaultRequestHeaders.IfModifiedSince = DateTimeOffset.UtcNow;
                var result = await Client.PostAsync(new Uri(uri), data);
                var stream = await result.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    html = reader.ReadToEnd();
                    return new Result(result.IsSuccessStatusCode, html, "", "", result.Headers.Location != null ? result.Headers.Location.OriginalString : "") ;
                }
            }
            catch (Exception ex)
            {
                var error = new Error("", ex.Message, ex.StackTrace, false);
                return new Result(false, html, JsonConvert.SerializeObject(error), "", uri);
            }
        }

        public async Task<Result> PostFormDataAsync(string uri, MultipartFormDataContent form) {
            var html = "";
            try
            {
                var result = await Client.PostAsync(new Uri(uri), form);
                var stream = await result.Content.ReadAsStreamAsync();
                using (var reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    html = reader.ReadToEnd();
                    var newResult = new Result(result.IsSuccessStatusCode, html)
                    {
                        RequestUri = result.RequestMessage.RequestUri.ToString()
                    };
                    return newResult;
                }
            }
            catch (Exception ex)
            {
                var error = new Error("", ex.Message, ex.StackTrace, false);
                return new Result(false, html, JsonConvert.SerializeObject(error), "", uri);
            }
        }
    }
}
