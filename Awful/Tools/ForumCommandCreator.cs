using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Awful.Tools
{
    public static class ForumCommandCreator
    {
        public static List<string> CreateForumCommand(string type, dynamic command)
        {
            var forumCommand = new ForumCommand()
            {
                Type = type,
                Command = command
            };
            return new List<string>() { JsonConvert.SerializeObject(forumCommand) };
        }
    }
}
