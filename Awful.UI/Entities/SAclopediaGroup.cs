using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.SAclopedia;

namespace Awful.UI.Entities
{
    public class SAclopediaGroup : List<SAclopediaEntryItem>
    {
        public string Name { get; private set; }

        public SAclopediaGroup(string name, List<SAclopediaEntryItem> entries)
            : base(entries)
        {
            this.Name = name;
        }
    }
}
