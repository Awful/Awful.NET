// <copyright file="SettingsAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Settings Action.
    /// </summary>
    public class SettingsAction
    {
        private IAwfulContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsAction"/> class.
        /// </summary>
        /// <param name="context">AwfulContext.</param>
        public SettingsAction(IAwfulContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Load default settings.
        /// </summary>
        /// <returns>Default Settings.</returns>
        public async Task<SettingOptions> LoadSettingOptionsAsync()
        {
            return this.context.GetAppSettings();
        }

        /// <summary>
        /// Save default settings.
        /// </summary>
        /// <param name="settingOptions">Setting Options to Save.</param>
        /// <returns>Setting Options.</returns>
        public async Task<SettingOptions> SaveSettingOptionsAsync(SettingOptions settingOptions)
        {
            this.context.SaveAppSettings(settingOptions);
            return settingOptions;
        }
    }
}
