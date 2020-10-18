// <copyright file="AwfulContext.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Awful.Database.Context
{
    /// <summary>
    /// Awful Database Context.
    /// </summary>
    public class AwfulContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulContext"/> class.
        /// </summary>
        /// <param name="databasePath">Path to the database.</param>
        public AwfulContext(string databasePath)
        {
            this.DatabasePath = databasePath;
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulContext"/> class.
        /// </summary>
        public AwfulContext()
        {
            this.DatabasePath = "Awful.sqlite";
            this.Database.EnsureCreated();
        }

        /// <summary>
        /// Gets or sets the users table.
        /// </summary>
        public DbSet<UserAuth> Users { get; set; }

        private string DatabasePath { get; set; }

        /// <summary>
        /// Add or update user.
        /// </summary>
        /// <param name="userAuth">The user auth.</param>
        /// <returns>Number of rows changed.</returns>
        public async Task<int> AddOrUpdateUser(UserAuth userAuth)
        {
            var user = this.Users.FirstOrDefault(node => node.UserAuthId == userAuth.UserAuthId);
            if (user == null)
            {
                await this.Users.AddAsync(userAuth).ConfigureAwait(false);
            }
            else
            {
                this.Users.Update(userAuth);
            }

            return await this.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
