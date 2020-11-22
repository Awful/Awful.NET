// <copyright file="BookmarkStarImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Awful.Mobile.UI.Tools.Converters
{
    /// <summary>
    /// Bookmark Star Image Converter.
    /// </summary>
    public class BookmarkStarImageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorited && isFavorited)
            {
                return new FontImageSource
                {
                    Glyph = FontAwesome.FontAwesomeIcons.Star,
                    FontFamily = "FontAwesomeSolid",
                    Color = Color.Yellow,
                    Size = 44,
                };
            }

            return new FontImageSource
            {
                Glyph = FontAwesome.FontAwesomeIcons.Star,
                FontFamily = "FontAwesomeRegular",
                Size = 44,
            };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
