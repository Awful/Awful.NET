using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Mazui.Tools
{
    public class Helpers
    {
        public static SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }

	public class CommandBarExtensions
	{
		public static readonly DependencyProperty HideMoreButtonProperty =
			DependencyProperty.RegisterAttached("HideMoreButton", typeof(bool), typeof(CommandBarExtensions),
				new PropertyMetadata(false, OnHideMoreButtonChanged));

		public static bool GetHideMoreButton(UIElement element)
		{
			if (element == null) throw new ArgumentNullException(nameof(element));
			return (bool)element.GetValue(HideMoreButtonProperty);
		}

		public static void SetHideMoreButton(UIElement element, bool value)
		{
			if (element == null) throw new ArgumentNullException(nameof(element));
			element.SetValue(HideMoreButtonProperty, value);
		}

		private static void OnHideMoreButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var commandBar = d as CommandBar;
			if (e == null || commandBar == null || e.NewValue == null) return;
			var morebutton = commandBar.FindDescendantByName("MoreButton");
			if (morebutton != null)
			{
				var value = GetHideMoreButton(commandBar);
				morebutton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
			}
			else
			{
				commandBar.Loaded += CommandBarLoaded;
			}
		}

		private static void CommandBarLoaded(object o, object args)
		{
			var commandBar = o as CommandBar;
			var morebutton = commandBar?.FindDescendantByName("MoreButton");
			if (morebutton == null) return;
			var value = GetHideMoreButton(commandBar);
			morebutton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
			commandBar.Loaded -= CommandBarLoaded;
		}
	}
}
