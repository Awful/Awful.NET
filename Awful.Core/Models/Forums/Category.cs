using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Awful.Parser.Models.Forums
{
    public class Category
    {
        public string Name { get; set; }

        public string Location { get; set; }

        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [JsonIgnore]
        public List<Forum> ForumList { get; set; } = new List<Forum>();
    }
}
