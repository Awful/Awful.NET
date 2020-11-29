// <copyright file="AwfulContainer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.ViewModels;
using Awful.UI.ViewModels;
using Awful.Webview;

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

            builder.RegisterType<TemplateHandler>().SingleInstance();
            builder.RegisterType<AwfulContext>();
            builder.RegisterType<AwfulViewModel>();
            builder.RegisterType<MobileSettingsPageViewModel>();
            builder.RegisterType<LepersPageViewModel>();
            builder.RegisterType<ForumsListPageViewModel>();
            builder.RegisterType<BookmarksPageViewModel>();
            builder.RegisterType<LoginPageViewModel>();
            builder.RegisterType<SAclopediaEntryListPageViewModel>();
            builder.RegisterType<SAclopediaEntryPageViewModel>();
            builder.RegisterType<ThreadReplyPageViewModel>();
            builder.RegisterType<ForumThreadListPageViewModel>();
            builder.RegisterType<ForumThreadPageViewModel>();
            builder.RegisterType<PrivateMessagesPageViewModel>();
            builder.RegisterType<PrivateMessagePageViewModel>();
            return builder.Build();
        }
    }
}
