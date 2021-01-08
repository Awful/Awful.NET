// <copyright file="/Users/drasticactions/Developer/Projects/Awful.NET/Awful.Mobile/Awful.Mobile/Tools/Converters/SubForumsImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// SubForums Image Converter.
    /// </summary>
    public class SubForumsImageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool showSubForums && showSubForums)
            {
                return new FontImageSource
                {
                    Glyph = FontAwesome.FontAwesomeIcons.MinusCircle,
                    FontFamily = "FontAwesomeSolid",
                    Color = Color.FromHex("#1483B1"),
                    Size = 44,
                };
            }

            return new FontImageSource
            {
                Glyph = FontAwesome.FontAwesomeIcons.ArrowDown,
                FontFamily = "FontAwesomeSolid",
                Size = 44,
                Color = Color.FromHex("#1483B1"),
            };
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}