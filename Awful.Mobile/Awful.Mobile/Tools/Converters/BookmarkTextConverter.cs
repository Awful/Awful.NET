// <copyright file="BookmarkTextConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Bookmark Text Converter.
    /// </summary>
    public class BookmarkTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            return (bool)value ? "Remove Bookmark" : "Add as Bookmark";
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
