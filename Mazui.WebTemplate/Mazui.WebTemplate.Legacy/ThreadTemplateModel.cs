using System;
using AwfulRedux.UI.Models.Threads;
using System.Collections.Generic;
using AwfulRedux.UI.Models.Posts;

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

