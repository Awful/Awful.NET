using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.Bans
{
    public class ProbationItem
    {
        public DateTime ProbationUntil { get; set; }

        public bool IsUnderProbation { get; set; }
    }
}
