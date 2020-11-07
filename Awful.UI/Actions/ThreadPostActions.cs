// <copyright file="ThreadPostActions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Awful.Core.Managers;
using Awful.Core.Managers.JSON;
using Awful.Core.Utilities;
using Awful.Database.Context;
using Awful.Database.Entities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;

namespace Awful.UI.Actions
{
    /// <summary>
    /// Thread Post Actions.
    /// </summary>
    public class ThreadPostActions
    {
        private AwfulContext context;
        private ThreadPostManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPostActions"/> class.
        /// </summary>
        /// <param name="client">AwfulClient.</param>
        /// <param name="context">AwfulContext.</param>
        public ThreadPostActions(AwfulClient client, AwfulContext context)
        {
            this.manager = new ThreadPostManager(client);
            this.context = context;
        }
    }
}
