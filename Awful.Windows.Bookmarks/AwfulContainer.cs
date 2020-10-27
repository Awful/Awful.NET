// <copyright file="AwfulContainer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Database.Context;
using Awful.UI.ViewModels;
using Awful.Webview;
using Awful.Windows.Bookmarks.ViewModels;

namespace Awful.UI
{
    /// <summary>
    /// Awful Container.
    /// </summary>
    public static class AwfulContainer
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

            builder.RegisterType<AwfulContext>().SingleInstance();
            builder.RegisterType<AwfulViewModel>();
            builder.RegisterType<SigninViewModel>();
            builder.RegisterType<BookmarksViewModel>();

            return builder.Build();
        }
    }
}
