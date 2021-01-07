// <copyright file="iOSPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Tools;
using Foundation;
using UIKit;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace Drastic.Forms.iOS
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    /// <summary>
    /// iOS Platform Properties.
    /// </summary>
#pragma warning disable SA1300 // Element should begin with upper-case letter
    public class iOSPlatformProperties : IPlatformProperties
#pragma warning restore SA1300 // Element should begin with upper-case letter
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

        /// <inheritdoc/>
        public string DatabasePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "drastic.db");

        /// <inheritdoc/>
        public string CookiePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "drastic.cookie");

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

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                viewController = window?.RootViewController ?? throw new InvalidOperationException("Could not find current view controller.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}