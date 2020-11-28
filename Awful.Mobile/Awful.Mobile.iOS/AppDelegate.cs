// <copyright file="AppDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Awful.Core.Tools;
using Foundation;
using UIKit;

namespace Awful.Mobile.iOS
{
    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the
    /// User Interface of the application, as well as listening (and optionally responding) to
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.

        /// <inheritdoc/>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SQLitePCL.Batteries.Init();
            global::Xamarin.Forms.Forms.Init();
            var builder = new ContainerBuilder();
            builder.RegisterType<iOSPlatformProperties>().As<IPlatformProperties>();
            this.LoadApplication(new App(builder));
            return base.FinishedLaunching(app, options);
        }
    }
}
