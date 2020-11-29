// <copyright file="TemplateHandler.cs" company="Drastic Actions">
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
using Awful.Webview.Entities.Themes;
using HandlebarsDotNet;

namespace Awful.Webview
{
    /// <summary>
    /// Handles creating SA HTML files for webviews.
    /// </summary>
    public class TemplateHandler
    {
        private readonly string threadHtml;
        private readonly string saclopediaHtml;
        private readonly string profileHtml;
        private readonly string banHtml;
        private readonly string postCss;
        private readonly string postDarkCss;
        private readonly string systemUiCss;
        private readonly string forumJs;
        private readonly string forumCss;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateHandler"/> class.
        /// </summary>
        public TemplateHandler()
        {
            this.threadHtml = TemplateHandler.GetResourceFileContentAsString("Templates.Thread.html.hbs");
            this.saclopediaHtml = TemplateHandler.GetResourceFileContentAsString("Templates.SAclopedia.html.hbs");
            this.profileHtml = TemplateHandler.GetResourceFileContentAsString("Templates.Profile.html.hbs");
            this.banHtml = TemplateHandler.GetResourceFileContentAsString("Templates.Ban.html.hbs");
            this.systemUiCss = TemplateHandler.GetResourceFileContentAsString("CSS.system-font.css");
            this.postCss = TemplateHandler.GetResourceFileContentAsString("CSS.posts-view.css");
            this.postDarkCss = TemplateHandler.GetResourceFileContentAsString("CSS.posts-view-dark.css");
            this.forumCss = TemplateHandler.GetResourceFileContentAsString("CSS.app.css");
            this.forumJs = TemplateHandler.GetResourceFileContentAsString("JS.forum.js");
        }

        /// <summary>
        /// Renders SAclopedia View.
        /// </summary>
        /// <param name="entry">SAclopedia Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderSAclopediaView(SAclopediaEntry entry, DefaultOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var template = Handlebars.Compile(this.saclopediaHtml);
            var entity = new SAclopedia() { Entry = entry };
            this.SetDefaults(entity, options);
            return template(entity);
        }

        /// <summary>
        /// Renders BanEntity View.
        /// </summary>
        /// <param name="entry">BanEntity Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderBanView(BanPage entry, DefaultOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var template = Handlebars.Compile(this.banHtml);
            var entity = new BanEntity() { Entry = entry };
            this.SetDefaults(entity, options);
            return template(entity);
        }

        /// <summary>
        /// Renders User Profile View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderProfileView(Awful.Core.Entities.JSON.User entry, DefaultOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var template = Handlebars.Compile(this.profileHtml);
            var entity = new ProfileEntity() { Entry = entry };
            this.SetDefaults(entity, options);
            return template(entity);
        }

        /// <summary>
        /// Renders Thread View.
        /// </summary>
        /// <param name="entry">User Entry.</param>
        /// <param name="options">Default Theme Options.</param>
        /// <returns>HTML Template String.</returns>
        public string RenderThreadPostView(Awful.Core.Entities.Threads.ThreadPost entry, DefaultOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var template = Handlebars.Compile(this.threadHtml);
            var entity = new ThreadPostEntity() { Entry = entry };
            this.SetDefaults(entity, options);
            return template(entity);
        }

        private static string GetResourceFileContentAsString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Awful.Webview." + fileName;

            string resource = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using StreamReader reader = new StreamReader(stream);
                resource = reader.ReadToEnd();
            }

            return resource;
        }

        private void SetDefaults(TemplateEntity entity, DefaultOptions options)
        {
            entity.CSS = new List<string>() { this.systemUiCss, this.forumCss};
            if (options.DeviceColorTheme == DeviceColorTheme.Dark)
            {
                entity.CSS.Add(this.postDarkCss);
            }
            else
            {
                entity.CSS.Add(this.postCss);
            }

            entity.JS = new List<string>() { this.forumJs };
            entity.DeviceColorTheme = options.DeviceColorTheme == DeviceColorTheme.Dark ? "theme-dark" : "theme";
            entity.Theme = options.DeviceTheme switch
            {
                DeviceTheme.Android => "md",
                DeviceTheme.iOS => "ios",
                _ => "aurora",
            };
        }
    }
}
