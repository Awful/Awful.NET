using Mazui.Core.Managers;
using Mazui.Tools.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.ViewModels
{
    public class ForumThreadListBaseViewModel : MazuiViewModel
    {
        public WebManager WebManager { get; set; }

        public async Task LoginUser()
        {
            WebManager = await UserHandler.GetDefaultAuthWebManager();
        }
    }
}
