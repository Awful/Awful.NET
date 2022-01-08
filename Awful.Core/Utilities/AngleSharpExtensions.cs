// <copyright file="AngleSharpExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AngleSharp.Dom;
using Awful.Core.Exceptions;

namespace Awful.Core
{
    /// <summary>
    /// Angle Sharp Extensions.
    /// </summary>
    public static class AngleSharpExtensions
    {
        /// <summary>
        /// Try Get Attribute.
        /// </summary>
        /// <param name="element"><see cref="IElement"/>.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="defaultOutput">Default Output.</param>
        /// <returns>string.</returns>
        public static string TryGetAttribute(this IElement element, string attribute, string defaultOutput = "")
        {
            if (element == null)
            {
                return defaultOutput;
            }

            var testOutput = element.GetAttribute(attribute);
            return testOutput ?? defaultOutput;
        }

        /// <summary>
        /// Get element via Query Selector.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="selector">Selector.</param>
        /// <returns>IElement.</returns>
        /// <exception cref="AwfulParserException">Failed to find element.</exception>
        public static IElement? GetElementViaQuerySelector(this IParentNode node, string selector)
        {
            var element = node.QuerySelector(selector);
            if (element == null)
            {
                throw new AwfulParserException($"Failed to find {selector} within {node.ToString()}");
            }

            return element;
        }
    }
}
