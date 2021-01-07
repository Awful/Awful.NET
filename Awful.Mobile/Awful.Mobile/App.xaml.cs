// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Autofac;
using Awful.Core.Tools;
using Awful.Database;
using Awful.Database.Entities;
using Awful.UI.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile
{
    /// <summary>
    /// Awful Application.
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
        /// Awful Application.
        /// </summary>
        /// <param name="builder">Container Builder.</param>
        public App(ContainerBuilder builder)
        {
            Device.SetFlags(new string[] { "MediaElement_Experimental", "Shell_UWP_Experimental", "AppTheme_Experimental", "CollectionView_Experimental", "Shapes_Experimental" });
            this.InitializeComponent();

            Container = AwfulContainerBuilder.BuildContainer(builder);
            var database = Container.Resolve<IDatabase>();
            var platform = Container.Resolve<IPlatformProperties>();
            var navigation = Container.Resolve<IAwfulNavigationHandler>();
            var settings = database.GetAppSettings();

            // If we're using the default system settings.
            if (settings.UseSystemThemeSettings)
            {
                if (platform.IsDarkTheme)
                {
                    ResourcesHelper.SetDarkMode();
                }
                else
                {
                    ResourcesHelper.SetLightMode();
                }
            }
            else
            {
                if (settings.CustomTheme != AppCustomTheme.None)
                {
                    ResourcesHelper.SetCustomTheme(settings.CustomTheme);
                }
                else
                {
                    if (settings.UseDarkMode)
                    {
                        ResourcesHelper.SetDarkMode();
                    }
                    else
                    {
                        ResourcesHelper.SetLightMode();
                    }
                }
            }

            navigation.SetMainAppPage();
#if DEBUG
            Xamarin.Forms.Xaml.Diagnostics.VisualDiagnostics.VisualTreeChanged += this.VisualDiagnostics_VisualTreeChanged;
#endif
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

        private void VisualDiagnostics_VisualTreeChanged(object sender, Xamarin.Forms.Xaml.Diagnostics.VisualTreeChangeEventArgs e)
        {
            var parentSourInfo = Xamarin.Forms.Xaml.Diagnostics.VisualDiagnostics.GetXamlSourceInfo(e.Parent);
            var childSourInfo = Xamarin.Forms.Xaml.Diagnostics.VisualDiagnostics.GetXamlSourceInfo(e.Child);
            System.Diagnostics.Debug.WriteLine($"VisualTreeChangeEventArgs {e.ChangeType}:" +
                $"{e.Parent}:{parentSourInfo?.SourceUri}:{parentSourInfo?.LineNumber}:{parentSourInfo?.LinePosition}-->" +
                $" {e.Child}:{childSourInfo?.SourceUri}:{childSourInfo?.LineNumber}:{childSourInfo?.LinePosition}");
        }
    }
}
