// <copyright file="TestHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        public static (string Username, string Password) GetDefaultAccountInfo()
        {
            if (AuthAccounts.AccountName.IsDefaultText())
            {
                Assert.Inconclusive("Account name is not set");
            }

            if (AuthAccounts.AccountPassword.IsDefaultText())
            {
                Assert.Inconclusive("Account password is not set");
            }

            return (AuthAccounts.AccountName, AuthAccounts.AccountPassword);
        }

        public static (string Username, string Password) GetPlatAccountInfo()
        {
            if (AuthAccounts.PlatAccountName.IsDefaultText())
            {
                Assert.Inconclusive("Plat Account name is not set");
            }

            if (AuthAccounts.PlatAccountPassword.IsDefaultText())
            {
                Assert.Inconclusive("Plat Account password is not set");
            }

            return (AuthAccounts.PlatAccountName, AuthAccounts.PlatAccountPassword);
        }

        public static (string Username, string Password) GetBannedAccountInfo()
        {
            if (AuthAccounts.BannedAccountName.IsDefaultText())
            {
                Assert.Inconclusive("Banned Account name is not set");
            }

            if (AuthAccounts.BannedAccountPassword.IsDefaultText())
            {
                Assert.Inconclusive("Banned Account password is not set");
            }

            return (AuthAccounts.BannedAccountName, AuthAccounts.BannedAccountPassword);
        }

        /// <summary>
        /// Checks if the text for the account info is the "default" or empty.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="text">Text.</param>
        /// <returns>Bool.</returns>
        public static bool IsDefaultText(this string text)
        {
            return string.IsNullOrEmpty(text) || text.Contains("{");
        }
    }
}
