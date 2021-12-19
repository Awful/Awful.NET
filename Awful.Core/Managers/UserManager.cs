// <copyright file="UserManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Handlers;
using Awful.Core.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Users on Something Awful.
    /// </summary>
    public class UserManager
    {
        private readonly AwfulClient webManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        public UserManager(AwfulClient webManager)
        {
            this.webManager = webManager;
        }

        /// <summary>
        /// Gets a user from their profile page.
        /// </summary>
        /// <param name="userId">The User Id. 0 gets the current logged in user.</param>
        /// <param name="token">A CancellationToken.</param>
        /// <returns>A User.</returns>
        public async Task<User> GetUserFromProfilePageAsync(long userId, CancellationToken token = default)
        {
            string url = string.Format(CultureInfo.InvariantCulture, EndPoints.UserProfile, userId);
            var result = await this.webManager.GetDataAsync(url, true, token).ConfigureAwait(false);
            try
            {
                var user = JsonSerializer.Deserialize<User>(result.ResultText);
                user.Result = result;
                return user;
            }
            catch (Exception ex)
            {
                throw new Awful.Core.Exceptions.AwfulParserException(ex, new Awful.Core.Entities.SAItem(result));
            }
        }
    }
}
