// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Autofac;
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
            this.InitializeComponent();

            AwfulContainerBuilder.BuildContainer(builder);
            Container = builder.Build();
            this.MainPage = new MainPage();
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
