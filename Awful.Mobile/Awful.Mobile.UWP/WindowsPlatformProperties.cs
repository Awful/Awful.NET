// <copyright file="WindowsPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;
using Windows.UI;
using Xamarin.Essentials;

namespace Awful.Mobile.UWP
{
    /// <summary>
    /// Awful Windows Platform.
    /// </summary>
    public class WindowsPlatformProperties : IPlatformProperties
    {
        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                var uiSettings = new Windows.UI.ViewManagement.UISettings();
                var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString(System.Globalization.CultureInfo.InvariantCulture);
                return color switch
                {
                    "#FF000000" => true,
                    "#FFFFFFFF" => false,
                    _ => false,
                };
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
        public void SetStatusBarColor(System.Drawing.Color color)
        {
        }

        /// <inheritdoc/>
        public Task<Stream> PickImageAsync()
        {
            var tcs = new TaskCompletionSource<Stream>();
            MainThread.BeginInvokeOnMainThread(async () => {
                var picker = new Windows.Storage.Pickers.FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary,
                };

                foreach (var ext in Awful.UI.Core.ImageUploadFileExtensions.ImageExtensions)
                {
                    picker.FileTypeFilter.Add(ext);
                }

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file == null)
                {
                    tcs.SetResult(null);
                    return;
                }

                var token = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                var fileStream = await file.OpenStreamForReadAsync().ConfigureAwait(false);
                tcs.SetResult(fileStream);
            });
            return tcs.Task;
        }
    }
}
