using System;
using Mazui.Core.Models.Posts;

namespace Mazui.WebTemplate.Legacy
{
	public class PrivateMessageTemplateModel
	{
		public Post PMPost { get; set; }

		public bool IsLoggedIn { get; set; }

		public bool IsDarkThemeSet { get; set; }
	}
}

