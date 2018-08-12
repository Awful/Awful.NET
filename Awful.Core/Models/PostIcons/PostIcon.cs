using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.PostIcons
{
    public class PostIcon
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; internal set; }
    }
}
