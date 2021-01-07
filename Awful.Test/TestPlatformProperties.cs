// <copyright file="TestPlatformProperties.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Awful.Core.Tools;

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
        public string CookiePath => $"{prefix}.test.cookie";

        /// <inheritdoc/>
        public string DatabasePath => $"{prefix}.awful.sqlite";

        /// <inheritdoc/>
        public bool IsDarkTheme => true;
    }
}
