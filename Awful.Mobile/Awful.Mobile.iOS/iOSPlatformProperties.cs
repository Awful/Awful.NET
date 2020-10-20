// <copyright file="iOSPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Awful.Core.Tools;
using Foundation;
using UIKit;

namespace Awful.Mobile.iOS
{
    public class iOSPlatformProperties : IPlatformProperties
    {
        public string CookiePath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cookie");

        public string DatabasePath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DB");
    }
}