// <copyright file="ICookieManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net;

namespace Awful
{
    /// <summary>
    /// Cookie Manager.
    /// </summary>
    public interface ICookieManager
    {
        /// <summary>
        /// Loads a cookie.
        /// </summary>
        /// <param name="path">Path to the cookie file.</param>
        /// <returns>A CookieContainer.</returns>
        CookieContainer LoadCookie(string path);

        /// <summary>
        /// Saves a cookie to storage.
        /// </summary>
        /// <param name="cookieContainer">The cookie container to be saved.</param>
        /// <param name="path">The path where the cookie should be saved.</param>
        void SaveCookie(CookieContainer cookieContainer, string path);

        /// <summary>
        /// Deleted a cookie.
        /// </summary>
        /// <param name="path">Path of the cookie.</param>
        void RemoveCookie(string path);
    }
}
