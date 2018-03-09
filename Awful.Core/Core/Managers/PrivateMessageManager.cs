using Awful.Models.Messages;
using Awful.Models.Posts;
using Awful.Models.Web;
using Awful.Parsers;
using Awful.Tools;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Managers
{
    public class PrivateMessageManager
    {
        private readonly WebManager _webManager;

        public PrivateMessageManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetPrivateMessagesAsync(int page)
        {
            Result result = new Result();
            try
            {
                var privateMessageEntities = new List<PrivateMessage>();
                var url = EndPoints.PrivateMessages;
                if (page > 0)
                {
                    url = EndPoints.PrivateMessages + string.Format(EndPoints.PageNumber, page);
                }

                result = await _webManager.GetDataAsync(url);
                Parsers.PrivateMessageHandler.Parse(privateMessageEntities, result.ResultHtml);
                result.ResultJson = JsonConvert.SerializeObject(privateMessageEntities);
                return result;
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
                return result;
            }
        }

        public async Task<Result> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity)
        {
            Result result = new Result();
            MultipartFormDataContent form;
            try
            {
                form = new MultipartFormDataContent
            {
                {new StringContent("dosend"), "action"},
                {new StringContent(newPrivateMessageEntity.Receiver), "touser"},
                {new StringContent(HtmlHelpers.HtmlEncode(newPrivateMessageEntity.Title)), "title"},
                {new StringContent(HtmlHelpers.HtmlEncode(newPrivateMessageEntity.Body)), "message"},
                {new StringContent("yes"), "parseurl"},
                {new StringContent("yes"), "parseurl"},
                {new StringContent("Send Message"), "submit"}
            };
                if (newPrivateMessageEntity.Icon != null)
                {
                    form.Add(new StringContent(newPrivateMessageEntity.Icon.Id.ToString(CultureInfo.InvariantCulture)), "iconid");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
                return result;
            }
            try
            {

                result = await _webManager.PostFormDataAsync(EndPoints.NewPrivateMessageBase, form);
                return result;
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
                return result;
            }
        }

        public async Task<Result> GetPrivateMessageAsync(string url)
        {
            Result result = new Result();
            try
            {
                result = await _webManager.GetDataAsync(url);
                List<Post> postList = new List<Post>();
                Parsers.PrivateMessageHandler.Parse(postList, result.ResultHtml);

                result.ResultJson = JsonConvert.SerializeObject(postList.First());
                return result;
            }
            catch (Exception ex)
            {
                ErrorHandler.CreateErrorObject(result, ex.Message, ex.StackTrace);
                return result;
            }
        }
    }
}
