using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.PostIcons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class PostIconManager
    {
        private readonly WebClient _webManager;

        public PostIconManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<List<PostIcon>> GetPostIconsAsync(bool isPrivateMessage = false, int forumId = 0)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");
            string url = isPrivateMessage ? EndPoints.NewPrivateMessage : string.Format(EndPoints.NewThread, forumId);
            var result = await _webManager.GetDataAsync(url);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return PostIconHandler.ParsePostIconList(document);
        }
    }
}
