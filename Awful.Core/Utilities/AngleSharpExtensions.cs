// <copyright file="AngleSharpExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using AngleSharp;
using AngleSharp.Dom;

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
    }
}