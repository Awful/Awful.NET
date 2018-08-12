using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.SAclopedia
{
    public class SAclopediaEntry
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<SAclopediaPost> Posts { get; set; } = new List<SAclopediaPost>();
    }

    public class SAclopediaPost
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string PostHtml { get; set; }

        public DateTime PostedDate { get; set; }
    }
}
