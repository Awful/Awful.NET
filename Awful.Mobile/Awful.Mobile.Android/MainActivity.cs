// <copyright file="MainActivity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Autofac;
using Awful.Core.Tools;
using FFImageLoading.Forms.Platform;

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Main Android Activity.
    /// </summary>
    [Activity(Label = "Awful", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Field, property, and method for Picture Picker.
        /// </summary>
        public static readonly int PickImageId = 1000;

        /// <summary>
        /// Gets or sets the Pick Image Task Completion Source.
        /// </summary>
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { get; set; }

        /// <summary>
        /// Gets the main activity instance.
        /// </summary>
        internal static MainActivity Instance { get; private set; }

        /// <inheritdoc/>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.DualScreen.DualScreenService.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();
            Forms9Patch.Droid.Settings.Initialize(this);
            var builder = new ContainerBuilder();
            builder.RegisterType<AndroidPlatformProperties>().As<IPlatformProperties>();
            this.LoadApplication(new App(builder));
        }

        /// <inheritdoc/>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = this.ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    this.PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    this.PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}