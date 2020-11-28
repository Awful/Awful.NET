// <copyright file="AndroidPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awful.Core.Tools;

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Android Platform Properties.
    /// </summary>
    public class AndroidPlatformProperties : IPlatformProperties
    {
        /// <summary>
        /// Gets the cookie path.
        /// </summary>
        public string CookiePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "awful.cookie");

        /// <summary>
        /// Gets the Database Path.
        /// </summary>
        public string DatabasePath => System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "awful.db");
    }
}
