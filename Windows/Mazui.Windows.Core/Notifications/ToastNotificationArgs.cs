using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazui.Notifications
{
	public class ToastNotificationArgs
	{
		public ToastType Type { get; set; }

		public long ThreadId { get; set; }

		public long ForumId { get; set; }

		public int PageNumber { get; set; }

		public bool IsThreadBookmark { get; set; }

		public bool OpenPrivateMessages { get; set; }

		public bool OpenBookmarks { get; set; }

		public bool OpenForum { get; set; }

		public bool IsLoggedIn { get; set; } = true;
	}

	public enum ToastType
	{
		Other,
		Ignore,
		Sleep,
		Toast
	}
}
