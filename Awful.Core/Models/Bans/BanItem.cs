using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.Bans
{

    public class BanPage
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<BanItem> Bans { get; set; } = new List<BanItem>();
    }
    public class BanItem
    {
        public string Type { get; set; }

        public int PostId { get; set; }

        public DateTime Date { get; set; }

        public string HorribleJerk { get; set; }

        public int HorribleJerkId { get; set; }

        public string PunishmentReason { get; set; }

        public string RequestedBy { get; set; }

        public int RequestedById { get; set; }

        public string ApprovedBy { get; set; }

        public int ApprovedById { get; set; }
    }
}
