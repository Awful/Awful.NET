using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace Awful.Web
{
    [AllowForWeb]
    public sealed class WebCommands
    {
        public bool HasReactLoaded { get; set; }

        public void reactLoaded()
        {
            HasReactLoaded = true;
        }
    }
}
