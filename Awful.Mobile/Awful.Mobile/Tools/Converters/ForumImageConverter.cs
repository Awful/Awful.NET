// <copyright file="ForumImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Awful.UI.Tools;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Forum Image Converter.
    /// </summary>
    public class ForumImageConverter : IValueConverter
    {
        private string[] imageNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumImageConverter"/> class.
        /// </summary>
        public ForumImageConverter()
        {
            this.imageNames = typeof(AwfulAsyncCommand).GetTypeInfo().Assembly.GetManifestResourceNames();
        }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var name = $"Awful.UI.ThreadTags.{value}.png";
            if (this.imageNames.Contains(name))
            {
                return ImageSource.FromResource(name, typeof(AwfulAsyncCommand).GetTypeInfo().Assembly);
            }

            return ImageSource.FromResource($"Awful.UI.ThreadTags.missing.png", typeof(AwfulAsyncCommand).GetTypeInfo().Assembly);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
