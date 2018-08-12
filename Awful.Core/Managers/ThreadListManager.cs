using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class ThreadListManager
    {
        private readonly WebClient _webManager;

        public ThreadListManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<List<Thread>> GetForumThreadListAsync(Forum forum, int page)
        {
            var pageUrl = string.Format(EndPoints.ForumPage, forum.ForumId) + string.Format(EndPoints.PageNumber, page);
            var result = await _webManager.GetDataAsync(pageUrl);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            ForumHandler.GetForumPageInfo(document, forum);
            return ThreadHandler.ParseForumThreadList(document, forum.ForumId);
        }
    }
}
