// <copyright file="IAwfulNavigation.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Database.Context;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Interfaces
{
    /// <summary>
    /// Awful Navigation.
    /// </summary>
    public interface IAwfulNavigation
    {
        /// <summary>
        /// Display Alert to User.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Message to user.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task DisplayAlertAsync(string title, string message);

        /// <summary>
        /// Close Modal Async.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        Task PopModalAsync();

        /// <summary>
        /// Push Modal Page to current navigation stack.
        /// If on tablet, pushes on top of Detail.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task PushModalAsync(object page);

        /// <summary>
        /// Push Page to current navigation stack.
        /// If on tablet, pushes on top of Master.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task PushPageAsync(object page);

        /// <summary>
        /// Set Detail Page for Master Detail if on Tablet, else push navigation.
        /// </summary>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task PushDetailPageAsync(object page);

        /// <summary>
        /// Refresh post page.
        /// Used on Thread/PM Post pages to callback
        /// to the last page.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        Task RefreshForumPageAsync();

        /// <summary>
        /// Refresh post page.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        Task RefreshPostPageAsync();

        /// <summary>
        /// Sets the main page of the application.
        /// Runs async on UI thread.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        Task SetMainAppPageAsync();

        /// <summary>
        /// Sets the main page of the application.
        /// Based on the device type.
        /// MainTabbedPage is the tabbed view.
        /// MainPage is a Flyout containing the MasterPage,
        /// And the detail page on the same screen.
        /// </summary>
        void SetMainAppPage();

        /// <summary>
        /// Setup the theme of the app on load.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        Task SetupThemeAsync();

        /// <summary>
        /// Set the theme of the app.
        /// </summary>
        /// <param name="theme">Theme.</param>
        void SetTheme(DeviceColorTheme theme);

        /// <summary>
        /// Display Prompt to User.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Message to user.</param>
        /// <param name="placeholder">Placeholder for message.</param>
        /// <param name="initialValue">Initial value for message.</param>
        /// <returns>String.</returns>
        Task<string> DisplayPromptAsync(string title, string message, string placeholder = "Text", string initialValue = "");

        /// <summary>
        /// Log out of SA.
        /// </summary>
        /// <param name="context">Awful Context.</param>
        /// <param name="properties">Awful Properties.</param>
        /// <returns>Task.</returns>
        Task LogoutAsync(IAwfulContext context, IPlatformProperties properties);
    }
}
