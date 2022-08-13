// <copyright file="PaywallHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Html.Dom;
using Awful.Exceptions;

namespace Awful.Handlers
{
    /// <summary>
    /// Paywall handler.
    /// </summary>
    public static class PaywallHandler
    {
        /// <summary>
        /// Checks if the current IHtmlDocument contains a paywall message.
        /// Throws a PaywallException if the content is paywalled.
        /// </summary>
        /// <param name="doc">IHtmlDocument containing the SA Page.</param>
        public static void CheckPaywall(IHtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var test = doc.QuerySelector(".inner");
            if (test != null)
            {
                if (test.TextContent.Contains("Sorry, you must be a registered forums member to view this page."))
                {
                    throw new PaywallException(Awful.Resources.ExceptionMessages.PaywallThreadHit);
                }
            }
        }
    }
}
