// <copyright file="AwfulViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;

namespace Awful.UI.ViewModels
{
    public class AwfulViewModel : BaseViewModel
    {
        private UserAuth user;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulViewModel"/> class.
        /// </summary>
        /// <param name="context">Awful Database Context.</param>
        public AwfulViewModel(AwfulContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.Context = context;
            this.user = context.GetDefaultUser();
            this.Client = new AwfulClient(this.user != null ? this.user.AuthCookies : new System.Net.CookieContainer());
        }

        public AwfulClient Client { get; set; }

        public AwfulContext Context { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public UserAuth CurrentUser
        {
            get { return this.user; }
            set { this.SetProperty(ref this.user, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the user is signed in.
        /// </summary>
        public bool IsSignedIn
        {
            get { return this.user != null; }
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed.
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.Context.Dispose();
                    this.Client.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
