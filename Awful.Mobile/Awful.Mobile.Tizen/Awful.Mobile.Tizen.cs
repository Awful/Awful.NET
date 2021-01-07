// <copyright file="Awful.Mobile.Tizen.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
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

            this.LoadApplication(new App());
        }

        private static void Main(string[] args)
        {
            using var app = new Program();
            Forms.Init(app);
            app.Run(args);
        }
    }
}
