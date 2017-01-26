using System;
using System.Collections.Generic;
using Mazui.Core.Models.Posts;
using Mazui.Core.Models.Threads;

namespace Mazui.WebTemplate.Legacy
{
	public class ThreadTemplateModel
	{
		public Thread ForumThread { get; set; }

		public List<Post> Posts { get; set; }

		public bool IsLoggedIn { get; set; }

		public bool IsDarkThemeSet { get; set; }

		public bool EmbeddedGifv {get; set;}

		public bool EmbeddedVideo {get; set;}

		public bool EmbeddedTweets {get; set;}
	}
}

