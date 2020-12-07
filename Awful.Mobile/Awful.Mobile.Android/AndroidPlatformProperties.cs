// <copyright file="AndroidPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Android Platform Properties.
    /// </summary>
    public class AndroidPlatformProperties : Core.Tools.IPlatformProperties
    {
        /// <inheritdoc/>
        public DeviceColorTheme GetTheme()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModeFlags = Xamarin.Essentials.Platform.CurrentActivity.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return DeviceColorTheme.Dark;

                    case UiMode.NightNo:
                        return DeviceColorTheme.Light;
                    default:
                        return DeviceColorTheme.Light;
                }
            }
            else
            {
                return DeviceColorTheme.Light;
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
    }
}
