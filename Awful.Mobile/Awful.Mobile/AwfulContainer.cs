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
            builder.RegisterType<MobileForumsListPageViewModel>();
            builder.RegisterType<MobileForumThreadListPageViewModel>();
            builder.RegisterType<MobileForumThreadPageViewModel>();
            builder.RegisterType<MobileSettingsPageViewModel>();
            builder.RegisterType<MobileLepersPageViewModel>();
            builder.RegisterType<MobileBookmarksPageViewModel>();
            builder.RegisterType<Awful.UI.ViewModels.LoginPageViewModel>();
            builder.RegisterType<MobileSAclopediaEntryListPageViewModel>();
            builder.RegisterType<MobileSAclopediaEntryPageViewModel>();
            builder.RegisterType<MobileThreadReplyPageViewModel>();
            builder.RegisterType<MobilePrivateMessagesPageViewModel>();
            builder.RegisterType<MobilePrivateMessagePageViewModel>();
            builder.RegisterType<MobileUserProfilePageViewModel>();
            builder.RegisterType<MobileNewThreadPageViewModel>();
            builder.RegisterType<MobileForumPostIconSelectionViewModel>();
            builder.RegisterType<MobilePostEditItemSelectionViewModel>();
            builder.RegisterType<Awful.UI.ViewModels.AcknowledgmentsPageViewModel>();
            builder.RegisterType<MobileNewPrivateMessagePageViewModel>();
            builder.RegisterType<MobileEmoteItemSelectionViewModel>();
            builder.RegisterType<AwfulPopup>().As<IAwfulPopup>().SingleInstance();
            builder.RegisterType<AwfulEditor>().As<IAwfulEditor>();
#if DEBUG
            builder.RegisterType<Awful.UI.ViewModels.DebugPageViewModel>();
#endif
            return builder.Build();
        }
    }
}
