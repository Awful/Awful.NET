using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Smilies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class SmileManager
    {
        private readonly WebClient _webManager;

        public SmileManager(WebClient webManager)
        {
            _webManager = webManager;
        }
        public async Task<List<SmileCategory>> GetSmileListAsync()
        {
            var result = await _webManager.GetDataAsync(EndPoints.SmileUrl);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return SmileHandler.ParseSmileList(document);
        }
    }
}
