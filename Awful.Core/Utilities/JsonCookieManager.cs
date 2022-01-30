// <copyright file="JsonCookieManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Net;

namespace Awful.Core.Utilities
{
    /// <summary>
    /// Cookie manager.
    /// </summary>
    public class JsonCookieManager : ICookieManager
    {
        /// <inheritdoc/>
        public CookieContainer LoadCookie(string path)
        {
            using FileStream stream = File.OpenRead(path);
            var b = new byte[stream.Length];
            stream.Read(b, 0, b.Length);
            var cookieCollection = Binary.ByteArrayToObject<CookieCollection>(b);
            var cookieContainer = new CookieContainer();
            if (cookieCollection != null)
            {
                cookieContainer.Add(cookieCollection);
            }

            return cookieContainer;
        }

        /// <inheritdoc/>
        public void RemoveCookie(string path)
        {
            System.IO.File.Delete(path);
        }

        /// <inheritdoc/>
        public void SaveCookie(CookieContainer cookieContainer, string path)
        {
            var bytes = Binary.ObjectToByteArray(cookieContainer.GetAllCookies());
            File.WriteAllBytes(path, bytes ?? new byte[0]);
        }
    }
}
