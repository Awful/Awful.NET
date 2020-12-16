// <copyright file="AwfulContainer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.Webview;

namespace Awful.ConsoleGUIApp
{
    public class AwfulContainer
    {
        /// <summary>
        /// Builds Awful Container.
        /// </summary>
        /// <param name="builder">Platform Specific Container.</param>
        /// <returns>IContainer.</returns>
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AwfulConsolePlatform>().As<IPlatformProperties>().SingleInstance();
            builder.RegisterType<AwfulLiteDBContext>().As<IAwfulContext>().SingleInstance();
            builder.RegisterType<AwfulErrorHandler>().As<IAwfulErrorHandler>().SingleInstance();
            //builder.RegisterType<AwfulWindowsNavigation>().As<IAwfulNavigation>().SingleInstance();
            builder.RegisterType<Awful.UI.ViewModels.LoginPageViewModel>();
            //builder.RegisterType<MainPageViewModel>();
            //builder.RegisterType<WindowsSettingsPageViewModel>();
            return builder.Build();
        }
    }
}
