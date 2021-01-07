// <copyright file="ITemplateHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Awful.Core.Entities.Bans;
using Awful.Core.Entities.JSON;
using Awful.Core.Entities.SAclopedia;
using Awful.Webview.Entities;

namespace Awful.Webview
{
    /// <summary>
    /// Template Handler.
    /// </summary>
    public interface ITemplateHandler
    {
        /// <summary>
        /// Renders SAclopedia View.
        /// </summary>
        /// <param name="entry">SAclopedia Entry.</param>
        /// <param name="isDarkMode">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderSAclopediaView(SAclopediaEntry entry, bool isDarkMode);

        /// <summary>
        /// Renders BanEntity View.
        /// </summary>
        /// <param name="entry">BanEntity Entry.</param>
        /// <param name="isDarkMode">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderBanView(BanPage entry, bool isDarkMode);

        /// <summary>
        /// Renders User Profile View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="isDarkMode">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderProfileView(Awful.Core.Entities.JSON.User entry, bool isDarkMode);

        /// <summary>
        /// Renders Thread View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="isDarkMode">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderThreadPostView(Awful.Core.Entities.Threads.ThreadPost entry, bool isDarkMode);

        /// <summary>
        /// Renders Acknowledgements View.
        /// </summary>
        /// <param name="isDarkMode">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderAcknowledgementstView(bool isDarkMode);
    }
}
