using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Parser.Models.Threads
{

    public class ThreadReply
    {
        public Thread Thread { get; set; }

        public int QuoteId { get; set; }

        public bool IsEdit { get; set; }
    }
}
