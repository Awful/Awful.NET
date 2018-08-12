using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awful.Parser.Models.Forums
{
    public class Forum
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int CurrentPage { get; set; }

        public bool IsSubForum { get; set; }

        [JsonIgnore]
        public List<Forum> SubForums { get; set; } = new List<Forum>();

        public int TotalPages { get; set; }

        [Key]
        public int ForumId { get; set; }

        [JsonIgnore]
        public virtual Forum ParentForum { get; set; }

        public int? ParentForumId { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }


        public bool IsBookmarks { get; set; }
        public int Order { get; set; }

        public int TotalTopics { get; set; }

        public int TotalPosts { get; set; }
    }
}
