using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Messages;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class PrivateMessageManager
    {
        private readonly WebClient _webManager;

        public PrivateMessageManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<List<PrivateMessage>> GetAllPrivateMessageListAsync()
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var pmList = new List<PrivateMessage>();
            var page = 0;
            while (true)
            {
                var result = await GetPrivateMessageListAsync(page);
                pmList.AddRange(result);
                if (!result.Any())
                    break;
                page++;
            }

            return pmList;
        }

        public async Task<List<PrivateMessage>> GetPrivateMessageListAsync(int page)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var url = EndPoints.PrivateMessages;
            if (page > 0)
            {
                url = EndPoints.PrivateMessages + string.Format(EndPoints.PageNumber, page);
            }

            var result = await _webManager.GetDataAsync(url);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return PrivateMessageHandler.ParseList(document);
        }

        public async Task<Post> GetPrivateMessageAsync(int id)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var pm = new PrivateMessage() { Id = id };
            await GetPrivateMessageAsync(pm);
            return pm.Post;
        }

        public async Task<Post> GetPrivateMessageAsync(PrivateMessage message)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            var result = await _webManager.GetDataAsync(EndPoints.PrivateMessages + $"?action=show&privatemessageid={message.Id}");
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            message.Post = PostHandler.ParsePost(document, document.Body);
            return message.Post;
        }

        public async Task<Result> SendPrivateMessageAsync(NewPrivateMessage newPrivateMessageEntity)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
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

    }
}
