using Awful.Parser.Models.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Awful.Tools
{
    public static class ResultChecker
    {
        public static async Task SendMessageDialogAsync(string message, bool isSuccess)
        {
            var title = isSuccess ? "Awful: " : "Awful - Error: ";
            var dialog = new MessageDialog((string.Concat(title, Environment.NewLine, Environment.NewLine, message)));
            await dialog.ShowAsync();
        }

        public static async Task<bool> CheckPaywallOrSuccess(Result result, bool showMessage = true)
        {
            if (result.IsSuccess)
                return true;
            if (result.Type == typeof(Error).ToString())
            {
                var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                if (error.IsPaywall)
                {
                    return false;
                }
            }
            if (showMessage)
                await SendMessageDialogAsync(result.Type + Environment.NewLine + result.ResultJson + Environment.NewLine, false);
            return false;
        }

        public static async Task<bool> CheckSuccess(Result result, bool showMessage = true)
        {
            if (result.IsSuccess)
                return true;
            if (showMessage)
                await SendMessageDialogAsync(result.Type + Environment.NewLine + result.ResultJson + Environment.NewLine, false);
            return false;
        }
    }
}
