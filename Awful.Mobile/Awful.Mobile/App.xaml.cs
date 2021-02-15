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
using Awful.UI.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.DualScreen;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful App.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Autofac Container.
        /// </summary>
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1401 // Fields should be private
        public static IContainer Container;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA2211 // Non-constant fields should not be visible

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="builder">Container Builder.</param>
        public App(ContainerBuilder builder)
        {
            Device.SetFlags(new string[] { "Shell_UWP_Experimental", "AppTheme_Experimental", "CollectionView_Experimental", "Shapes_Experimental" });
            this.InitializeComponent();
            if (Container == null)
            {
                Container = Awful.UI.AwfulContainer.BuildContainer(builder);
            }

            var platformConfig = App.Container.Resolve<IPlatformProperties>();
            var database = App.Container.Resolve<IAwfulContext>();
            var navigation = App.Container.Resolve<IAwfulNavigation>();
            StartApp(platformConfig, navigation);
        }

        /// <summary>
        /// Starts App.
        /// </summary>
        /// <param name="platformProperties">Platform Properties.</param>
        /// <param name="navigation">Navigation Handler.</param>
        public static void StartApp(IPlatformProperties platformProperties, IAwfulNavigation navigation)
        {
            if (platformProperties == null)
            {
                throw new ArgumentNullException(nameof(platformProperties));
            }

            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var user = System.IO.File.Exists(platformProperties.CookiePath);
            if (!user)
            {
                App.Current.MainPage = new LoginPage();
            }
            else
            {
                navigation.SetMainAppPage();
            }
        }

        /// <summary>
        /// Gets the current pages background color.
        /// </summary>
        /// <returns>Xamarin Forms Color.</returns>
        public static Xamarin.Forms.Color GetCurrentBackgroundColor()
        {
            return App.Current.MainPage.BackgroundColor;
        }

        /// <summary>
        /// Gets an application resource.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <returns>Resource.</returns>
        public static object GetApplicationResource(string name)
        {
           return App.Current.Resources[name];
        }

        /// <inheritdoc/>
        protected override void OnStart()
        {
        }

        /// <inheritdoc/>
        protected override void OnSleep()
        {
        }

        /// <inheritdoc/>
        protected override void OnResume()
        {
        }
    }
}
