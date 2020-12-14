using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Win.Controls
{
    /// <summary>
    /// Awful Menu Category.
    /// </summary>
    public class AwfulMenuCategory
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category icon.
        /// </summary>
        public string CategoryIcon { get; set; }

        /// <summary>
        /// Gets or sets the menu page.
        /// </summary>
        public Type Page { get; set; }
    }
}
