// <copyright file="AndroidPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Android Platform Properties.
    /// </summary>
    public class AndroidPlatformProperties : Core.Tools.IPlatformProperties
    {
        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
                {
                    var uiModeFlags = Xamarin.Essentials.Platform.CurrentActivity.Resources.Configuration.UiMode & UiMode.NightMask;

                    switch (uiModeFlags)
                    {
                        case UiMode.NightYes:
                            return true;

                        case UiMode.NightNo:
                            return false;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
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

        /// <inheritdoc/>
        public void SetStatusBarColor(Color color)
        {
            Xamarin.Essentials.Platform.CurrentActivity.Window.SetStatusBarColor(color.ToPlatformColor());
        }

        /// <inheritdoc/>
        public Task<Stream> PickImageAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }
    }
}
