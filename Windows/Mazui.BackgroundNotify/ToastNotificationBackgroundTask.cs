using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.SettingsService;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace Mazui.BackgroundNotify
{
	public sealed class ToastNotificationBackgroundTask : IBackgroundTask
	{

		public void Run(IBackgroundTaskInstance taskInstance)
		{
			BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
			Template10.Services.SettingsService.ISettingsHelper _helper = new SettingsHelper();

			var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
			var arguments = details?.Argument;
			if (arguments == "sleep")
			{
				_helper.Write("BookmarkNotifications", false);
			}
			deferral.Complete();
		}
	}
}
