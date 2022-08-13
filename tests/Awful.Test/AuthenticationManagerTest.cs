// <copyright file="AuthenticationManagerTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Awful.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Awful.Test
{
    /// <summary>
    /// Authentication Manager Tests.
    /// </summary>
    [TestClass]
    public class AuthenticationManagerTest
    {
        /// <summary>
        /// Can Authenticate Default Accounts.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task CanAuthenticateDefaultAccount()
        {
            var authManager = new AuthenticationManager(new AwfulClient());
            var (username, password) = TestHelpers.GetDefaultAccountInfo();
            var auth = await authManager.AuthenticateAsync(username, password);
            Assert.IsNotNull(auth);
            Assert.IsTrue(auth.IsSuccess);
            Assert.IsTrue(auth.AuthenticationCookieContainer.Count > 1);
        }
    }
}
