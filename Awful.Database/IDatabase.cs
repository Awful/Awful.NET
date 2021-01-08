// <copyright file="IDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Entities.JSON;
using Awful.Database.Entities;

namespace Awful.Database
{
    /// <summary>
    /// IDatabase.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets a value indicating whether there are logged in users.
        /// </summary>
        /// <returns>Bool.</returns>
        bool IsUserLoggedIn { get; }

        /// <summary>
        /// Gets app settings.
        /// </summary>
        /// <returns>App Settings.</returns>
        SettingOptions GetAppSettings();

        /// <summary>
        /// Save App Settings.
        /// </summary>
        /// <param name="appSettings">App Settings.</param>
        /// <returns>Bool if saved.</returns>
        bool SaveAppSettings(SettingOptions appSettings);

        /// <summary>
        /// Gets forums list.
        /// </summary>
        /// <returns>List of Forums.</returns>
        List<Forum> GetForums();

        /// <summary>
        /// Save Forums List.
        /// </summary>
        /// <param name="forums">Forums.</param>
        /// <returns>Bool if saved.</returns>
        bool SaveForums(List<Forum> forums);

        /// <summary>
        /// Gets the current logged in user.
        /// </summary>
        /// <returns>UserAuth.</returns>
        UserAuth GetLoggedInUser();

        /// <summary>
        /// Saves the current logged in user.
        /// </summary>
        /// <param name="userAuth">UserAuth.</param>
        /// <returns>Bool if saved.</returns>
        bool SaveLoggedInUser(UserAuth userAuth);
    }
}
