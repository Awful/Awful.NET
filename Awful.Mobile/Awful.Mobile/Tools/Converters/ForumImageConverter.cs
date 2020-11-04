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
using Xamarin.Forms;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Forum Image Converter.
    /// </summary>
    public class ForumImageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return ImageSource.FromResource($"Awful.Mobile.ThreadTags.{value}.png", typeof(ForumImageConverter).GetTypeInfo().Assembly);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
