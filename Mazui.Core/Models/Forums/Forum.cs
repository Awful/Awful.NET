using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mazui.Core.Models.Forums
{
    public class Forum
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int CurrentPage { get; set; }

        public bool IsSubforum { get; set; }

        public int TotalPages { get; set; }

        [Key]
        public int ForumId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public bool IsBookmarks { get; set; }
        public int Order { get; set; }
    }
}
