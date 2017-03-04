using Mazui.Core.Models.PostIcons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazui.Core.Models.Messages
{
    public class NewPrivateMessage
    {
        public PostIcon Icon { get; set; }

        public string Title { get; set; }

        public string Receiver { get; set; }

        public string Body { get; set; }
    }

}
