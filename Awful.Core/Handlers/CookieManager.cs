// <copyright file="CookieManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace Awful.Core.Handlers
{
    /// <summary>
    /// Helpers for managing cookies, used by Something Awful for authentication.
    /// </summary>
    public static class CookieManager
    {
        /// <summary>
        /// Loads a cookie.
        /// </summary>
        /// <param name="path">Path to the cookie file.</param>
        /// <returns>A CookieContainer.</returns>
        public static CookieContainer LoadCookie(string path)
        {
            using FileStream stream = File.OpenRead(path);
            var formatter = new BinaryFormatter();
            return (CookieContainer)formatter.Deserialize(stream);
        }

        /// <summary>
        /// Saves a cookie to storage.
        /// </summary>
        /// <param name="cookieContainer">The cookie container to be saved.</param>
        /// <param name="path">The path where the cookie should be saved.</param>
        public static void SaveCookie(CookieContainer cookieContainer, string path)
        {
            using FileStream stream = File.Create(path);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, cookieContainer);
        }

        /// <summary>
        /// Deleted a cookie.
        /// </summary>
        /// <param name="path">Path of the cookie.</param>
        public static void RemoveCookie(string path)
        {
            System.IO.File.Delete(path);
        }
    }
}
