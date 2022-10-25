// <copyright file="SettingsAction.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.UI.Entities;
using Awful.UI.Services;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Settings Action.
    /// </summary>
    public class SettingsAction
    {
        private IDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsAction"/> class.
        /// </summary>
        /// <param name="context">AwfulContext.</param>
        public SettingsAction(IDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Load default settings.
        /// </summary>
        /// <returns>Default Settings.</returns>
        public async Task<SettingOptions> LoadSettingOptionsAsync()
        {
            return context.GetAppSettings();
        }

        /// <summary>
        /// Save default settings.
        /// </summary>
        /// <param name="settingOptions">Setting Options to Save.</param>
        /// <returns>Setting Options.</returns>
        public async Task<SettingOptions> SaveSettingOptionsAsync(SettingOptions settingOptions)
        {
            context.SaveAppSettings(settingOptions);
            return settingOptions;
        }
    }
}
