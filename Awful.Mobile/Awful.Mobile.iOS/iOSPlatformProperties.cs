// <copyright file="iOSPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;
using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Awful.Mobile.iOS
{
    /// <summary>
    /// iOS Platform Properties.
    /// </summary>
    public class iOSPlatformProperties : IPlatformProperties
    {
        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                var result = Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync<bool>(() =>
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                    {
                        var currentUIViewController = GetVisibleViewController();

                        if (currentUIViewController == null)
                        {
                            return false;
                        }

                        var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                        switch (userInterfaceStyle)
                        {
                            case UIUserInterfaceStyle.Light:
                                return false;
                            case UIUserInterfaceStyle.Dark:
                                return true;
                            default:
                                throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
                        }
                    }
                    else
                    {
                        return false;
                    }
                });
                return result.Result;
            }
        }

        /// <summary>
        /// Gets the cookie path.
        /// </summary>
        public string CookiePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "awful.cookie");

        /// <summary>
        /// Gets the Database Path.
        /// </summary>
        public string DatabasePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "awful.db");

        private static UIViewController GetVisibleViewController()
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
            {
                return null;
            }

            if (window.WindowLevel == UIWindowLevel.Normal)
            {
                viewController = window.RootViewController;
            }

            if (viewController is null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                viewController = window?.RootViewController ?? throw new InvalidOperationException("Could not find current view controller.");
            }

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }

        /// <inheritdoc/>
        public void SetStatusBarColor(System.Drawing.Color color)
        {
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(
            new NSString("statusBar")) as UIView;
            if (statusBar != null && statusBar.RespondsToSelector(
            new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = color.ToPlatformColor();
            }
        }
    }
}