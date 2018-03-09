using HtmlAgilityPack;
using Awful.Models.Smilies;
using Awful.Models.Web;
using Awful.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Awful.Parsers;

namespace Awful.Managers
{
    public class SmileManager
    {
        private readonly WebManager _webManager;

        public SmileManager(WebManager webManager)
        {
            _webManager = webManager;
        }

        public async Task<Result> GetSmileListAsync()
        {
            var html = "";
            try
            {
                var smileCategoryList = new List<SmileCategory>();
                var result = await _webManager.GetDataAsync(EndPoints.SmileUrl);
                html = result.ResultHtml;
                SmileHandler.Parse(smileCategoryList, result.ResultHtml);
                return new Result(true, result.ResultHtml, JsonConvert.SerializeObject(smileCategoryList));
            }
            catch (Exception ex)
            {
                return ErrorHandler.CreateErrorObject(new Result(false, html), ex.Message, ex.StackTrace);
            }
        }

    }
}
