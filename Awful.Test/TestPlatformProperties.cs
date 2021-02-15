// <copyright file="TestPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Tools;
using Awful.Webview.Entities.Themes;

namespace Awful.Test
{
    /// <summary>
    /// Test Platform Properties.
    /// </summary>
    public class TestPlatformProperties : IPlatformProperties
    {
        private string prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPlatformProperties"/> class.
        /// </summary>
        /// <param name="prefix">Prefix to the file names.</param>
        public TestPlatformProperties(string prefix)
        {
            this.prefix = prefix;
            if (File.Exists(this.DatabasePath))
            {
                File.Delete(this.DatabasePath);
            }
        }

        /// <inheritdoc/>
        public string CookiePath => $"{this.prefix}.test.cookie";

        /// <inheritdoc/>
        public string DatabasePath => $"{this.prefix}.awful.sqlite";

        /// <inheritdoc/>
        public bool IsDarkTheme => true;

        public Task<Stream> PickImageAsync()
        {
            throw new NotImplementedException();
        }

        public void SetStatusBarColor(Color color)
        {
            throw new NotImplementedException();
        }
    }
}
