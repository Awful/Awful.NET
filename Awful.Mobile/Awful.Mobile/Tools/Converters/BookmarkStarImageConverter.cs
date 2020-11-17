// <copyright file="BookmarkStarImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Awful.Mobile.UI.Tools.Converters
{
    public class BookmarkStarImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorited && isFavorited)
            {
                return new FontImageSource
                {
                    Glyph = "",
                    FontFamily = "FontAwesomeSolid",
                    Size = 44
                };
            }

            return new FontImageSource
            {
                Glyph = "",
                FontFamily = "FontAwesomeRegular",
                Size = 44
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
