// <copyright file="AwfulContainerBuilder.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Database;
using Awful.Database.Context;
using Awful.Mobile.Tools;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
using Awful.Webview;

namespace Awful.Mobile
{
    /// <summary>
    /// Builds Autofac Container.
    /// </summary>
    public static class AwfulContainerBuilder
    {
        /// <summary>
        /// Builds Awful Container.
        /// </summary>
        /// <param name="builder">Platform Specific Container.</param>
        /// <returns>IContainer.</returns>
        public static IContainer BuildContainer(ContainerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<AwfulDatabase>().As<IDatabase>().SingleInstance();
            builder.RegisterType<AwfulNavigationHandler>().As<IAwfulNavigationHandler>().SingleInstance();
            builder.RegisterType<AwfulErrorHandler>().As<IAwfulErrorHandler>().SingleInstance();

            builder.RegisterType<SettingsViewModel>();

            return builder.Build();
        }
    }
}
