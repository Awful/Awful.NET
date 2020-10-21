// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful App Startup.
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container;

        public App(ContainerBuilder builder)
        {
            this.InitializeComponent();
            Container = Awful.UI.AwfulContainer.BuildContainer(builder);
            this.MainPage = new AwfulShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
