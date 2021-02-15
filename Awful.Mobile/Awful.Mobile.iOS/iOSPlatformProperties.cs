// <copyright file="iOSPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        TaskCompletionSource<Stream> taskCompletionSource;
        UIImagePickerController imagePicker;

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
            if (UIApplication.SharedApplication.KeyWindow == null)
            {
                return;
            }

            UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
            statusBar.BackgroundColor = color.ToPlatformColor();
            UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
        }

        /// <inheritdoc/>
        public System.Threading.Tasks.Task<Stream> PickImageAsync()
        {
            // Create and define UIImagePickerController
            this.imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary),
            };

            // Set event handlers
            this.imagePicker.FinishedPickingMedia += this.OnImagePickerFinishedPickingMedia;
            this.imagePicker.Canceled += this.OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentViewController(imagePicker, true, null);

            // Return Task object
            this.taskCompletionSource = new TaskCompletionSource<Stream>();
            return this.taskCompletionSource.Task;
        }

        private void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                // Convert UIImage to .NET Stream object
                NSData data;
                if (args.ReferenceUrl.PathExtension.Equals("PNG") || args.ReferenceUrl.PathExtension.Equals("png"))
                {
                    data = image.AsPNG();
                }
                else
                {
                    data = image.AsJPEG(1);
                }

                Stream stream = data.AsStream();

                this.UnregisterEventHandlers();

                // Set the Stream as the completion of the Task
                this.taskCompletionSource.SetResult(stream);
            }
            else
            {
                this.UnregisterEventHandlers();
                this.taskCompletionSource.SetResult(null);
            }

            this.imagePicker.DismissModalViewController(true);
        }

        private void OnImagePickerCancelled(object sender, EventArgs args)
        {
            this.UnregisterEventHandlers();
            this.taskCompletionSource.SetResult(null);
            this.imagePicker.DismissModalViewController(true);
        }

        private void UnregisterEventHandlers()
        {
            this.imagePicker.FinishedPickingMedia -= this.OnImagePickerFinishedPickingMedia;
            this.imagePicker.Canceled -= this.OnImagePickerCancelled;
        }
    }
}