// <copyright file="Awful.Mobile.Tizen.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Autofac;
using Awful.Core.Tools;
using Xamarin.Forms;

namespace Awful.Mobile
{
    /// <summary>
    /// Tizen Program.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    internal class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <inheritdoc/>
        protected override void OnCreate()
        {
            base.OnCreate();
            var container = new ContainerBuilder();
            container.RegisterType<TizenPlatformProperties>().As<IPlatformProperties>();
            this.LoadApplication(new App(container));
        }

        private static void Main(string[] args)
        {
            using var app = new Program();
            Forms.Init(app);
            app.Run(args);
        }
    }
}
