﻿// <copyright file="UserManager.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.JSON;
using Awful.Core.Handlers;
using Awful.Core.Utilities;

namespace Awful.Core.Managers
{
    /// <summary>
    /// Manager for handling Users on Something Awful.
    /// </summary>
    public class UserManager
    {
        private readonly AwfulClient webManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="webManager">The SA WebClient.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public UserManager(AwfulClient webManager, ILogger logger)
        {
            this.webManager = webManager;
            this.logger = logger;
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
                if (user == null)
                {
                    throw new Awful.Core.Exceptions.AwfulParserException("Failed to parse user in GetUserFromProfilePageAsync", new Awful.Core.Entities.SAItem(result));
                }

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
