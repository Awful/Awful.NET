// <copyright file="IPlatformServices.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing;

namespace Awful.UI.Services
{
    /// <summary>
    /// Platform Properties.
    /// </summary>
    public interface IPlatformServices
    {
        /// <summary>
        /// Gets a value indicating whether the current platform is running a system level dark theme.
        /// </summary>
        bool IsDarkTheme { get; }

        /// <summary>
        /// Gets the path to where cookies are stored.
        /// </summary>
        string CookiePath { get; }

        /// <summary>
        /// Gets the path to where the database is stored.
        /// </summary>
        string DatabasePath { get; }

        /// <summary>
        /// Gets the cookie manager.
        /// </summary>
        public ICookieManager CookieManager { get; }

        /// <summary>
        /// Sets the status bar color.
        /// </summary>
        /// <param name="color">Color to set the status bar to.</param>
        void SetStatusBarColor(Color color);

        /// <summary>
        /// Gets an image.
        /// </summary>
        /// <returns>Image Stream.</returns>
        public Task<Stream> PickImageAsync();
    }
}
