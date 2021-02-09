// <copyright file="AwfulContainer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Database.Context;
using Awful.Mobile.Controls;
using Awful.Mobile.ViewModels;
using Awful.UI.Interfaces;
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

            builder.RegisterType<MobileTemplateHandler>().As<ITemplateHandler>().SingleInstance();
            builder.RegisterType<AwfulLiteDBContext>().As<IAwfulContext>().SingleInstance();
            builder.RegisterType<AwfulMobileNavigation>().As<IAwfulNavigation>().SingleInstance();
            builder.RegisterType<AwfulMobileErrorHandler>().As<IAwfulErrorHandler>().SingleInstance();
            builder.RegisterType<MainTabbedPageViewModel>();
            builder.RegisterType<ForumsListPageViewModel>();
            builder.RegisterType<ForumThreadListPageViewModel>();
            builder.RegisterType<ForumThreadPageViewModel>();
            builder.RegisterType<SettingsPageViewModel>();
            builder.RegisterType<LepersPageViewModel>();
            builder.RegisterType<BookmarksPageViewModel>();
            builder.RegisterType<Awful.UI.ViewModels.LoginPageViewModel>();
            builder.RegisterType<SAclopediaEntryListPageViewModel>();
            builder.RegisterType<SAclopediaEntryPageViewModel>();
            builder.RegisterType<ThreadReplyPageViewModel>();
            builder.RegisterType<PrivateMessagesPageViewModel>();
            builder.RegisterType<PrivateMessagePageViewModel>();
            builder.RegisterType<UserProfilePageViewModel>();
            builder.RegisterType<NewThreadPageViewModel>();
            builder.RegisterType<ForumPostIconSelectionViewModel>();
            builder.RegisterType<PostEditItemSelectionViewModel>();
            builder.RegisterType<Awful.UI.ViewModels.AcknowledgmentsPageViewModel>();
            builder.RegisterType<NewPrivateMessagePageViewModel>();
            builder.RegisterType<EmoteItemSelectionViewModel>();
            builder.RegisterType<AwfulPopup>().As<IAwfulPopup>().SingleInstance();
            builder.RegisterType<AwfulEditor>().As<IAwfulEditor>();
#if DEBUG
            builder.RegisterType<Awful.UI.ViewModels.DebugPageViewModel>();
#endif
            return builder.Build();
        }
    }
}
