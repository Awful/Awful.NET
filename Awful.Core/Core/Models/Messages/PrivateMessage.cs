using Awful.Models.PostIcons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Models.Messages
{
    public class PrivateMessage
    {
        public string Status { get; set; }

        public PostIcon Icon { get; set; }

        public string ImageIconLocation { get; set; }

        public string Title { get; set; }

        public string Sender { get; set; }

        public string Date { get; set; }

        public string MessageUrl { get; set; }
    }
}
