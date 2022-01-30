// <copyright file="TestHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Awful.Test
{
    /// <summary>
    /// Test Helpers.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Gets the sample HTML file from storage.
        /// </summary>
        /// <param name="filename">The sample filename.</param>
        /// <returns>HTML sample.</returns>
        /// <exception cref="NullReferenceException">Thrown if sample doesn't exist.</exception>
        public static string GetSampleHtmlFile(string filename)
        {
            var baseLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (baseLocation is null)
            {
                throw new NullReferenceException("Base Location is null");
            }

            var filePath = System.IO.Path.Combine(baseLocation, "SampleHtml", filename);
            if (!System.IO.File.Exists(filePath))
            {
                throw new NullReferenceException("File is null");
            }

            return System.IO.File.ReadAllText(filePath);
        }
    }
}
