// <copyright file="BookmarksThreadHeaderColorConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Bookmark Thread Header Color Converter.
    /// </summary>
    public class BookmarksThreadHeaderColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = (string)value;
            switch (item)
            {
                case "bm0":
                    return new SolidColorBrush(Colors.Orange);
                case "bm1":
                    return new SolidColorBrush(Colors.Red);
                case "bm2":
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.Orange);
            }
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
