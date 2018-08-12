using Awful.Parser.Core;
using Awful.Parser.Handlers;
using Awful.Parser.Models.Bans;
using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Parser.Managers
{
    public class BanManager
    {
        private readonly WebClient _webManager;

        public BanManager(WebClient webManager)
        {
            _webManager = webManager;
        }

        public async Task<BanPage> GetBanPageAsync(int page = 1)
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");

            var result = await _webManager.GetDataAsync(string.Format(EndPoints.RapSheet, page));
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            return BanHandler.ParseBanPage(document);
        } 

        public async Task<ProbationItem> CheckForProbation()
        {
            if (!_webManager.IsAuthenticated)
                throw new Exception("User must be authenticated before using this method.");

            var result = await _webManager.GetDataAsync(EndPoints.BaseUrl);
            var document = await _webManager.Parser.ParseAsync(result.ResultHtml);
            var prob = BanHandler.ParseForProbation(document);
            _webManager.Probation = prob;
            return prob;
        }
    }
}
