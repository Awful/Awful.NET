using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.Smilies
{
    public class SmileCategory
    {
        public SmileCategory()
        {
            SmileList = new List<Smile>();
        }

        public List<Smile> SmileList { get; set; } = new List<Smile>();

        public string Name { get; set; }
    }

    public class Smile
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }
        public string ImageLocation { get; set; }
    }
}
