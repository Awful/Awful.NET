// <copyright file="ImageResourceExtension.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Tools.Markup
{
    /// <summary>
    /// Image Resource Extension.
    /// Used to get values out of assembly files.
    /// </summary>
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        /// <summary>
        /// Gets or sets the source of the file.
        /// </summary>
        public string Source { get; set; }

        /// <inheritdoc/>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(this.Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
