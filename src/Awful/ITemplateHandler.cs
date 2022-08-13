// <copyright file="ITemplateHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Awful.Entities.Bans;
using Awful.Entities.SAclopedia;
using Awful.Themes;

namespace Awful
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
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderSAclopediaView(SAclopediaEntry entry, DefaultOptions options);

        /// <summary>
        /// Renders BanEntity View.
        /// </summary>
        /// <param name="entry">BanEntity Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderBanView(BanPage entry, DefaultOptions options);

        /// <summary>
        /// Renders User Profile View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderProfileView(Awful.Entities.JSON.User entry, DefaultOptions options);

        /// <summary>
        /// Renders Thread View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderThreadPostView(Awful.Entities.Threads.ThreadPost entry, DefaultOptions options);

        /// <summary>
        /// Renders Acknowledgements View.
        /// </summary>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderAcknowledgementstView(DefaultOptions options);
    }
}