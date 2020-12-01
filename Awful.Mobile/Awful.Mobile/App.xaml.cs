// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Mobile.Pages;
using Awful.Mobile.ViewModels;
using Awful.UI.Actions;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful App.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="builder">Container Builder.</param>
        public App(ContainerBuilder builder)
        {
            Device.SetFlags(new string[] { "Shell_UWP_Experimental", "AppTheme_Experimental", "CollectionView_Experimental", "Shapes_Experimental" });
            this.InitializeComponent();
            Container = Awful.UI.AwfulContainer.BuildContainer(builder);

            var platformConfig = App.Container.Resolve<IPlatformProperties>();
            MobileAwfulViewModel.StartApp(platformConfig);
        }

        /// <summary>
        /// Autofac Container.
        /// </summary>
        public static IContainer Container;

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
