using Awful.Parser.Models.PostIcons;
using Awful.Parser.Models.Posts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.Messages
{
    public class PrivateMessage
    {
        public int Id { get; set; }

        public PostIcon Icon { get; set; }

        public string ImageIconLocation { get; set; }

        public string Title { get; set; }

        public string Sender { get; set; }

        public DateTime Date { get; set; }

        public Post Post { get; set; }

        public string MessageUrl { get; set; }
        public string ImageIconUrl { get; internal set; }
        public string StatusImageIconUrl { get; internal set; }
        public string StatusImageIconLocation { get; internal set; }
    }
}
