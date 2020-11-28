// <copyright file="MainActivity.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Autofac;
using Awful.Core.Tools;

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Main Android Activity.
    /// </summary>
    [Activity(Label = "Awful.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
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

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            var builder = new ContainerBuilder();
            builder.RegisterType<AndroidPlatformProperties>().As<IPlatformProperties>();
            this.LoadApplication(new App(builder));
        }
    }
}