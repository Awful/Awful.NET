// <copyright file="ShowSubforumsConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Show Subforums Converter.
    /// </summary>
    public class ShowSubforumsConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isShowSubforums && !isShowSubforums)
            {
                return new FontImageSource
                {
                    Glyph = FontAwesome.FontAwesomeIcons.Plus,
                    FontFamily = "FontAwesomeSolid",
                    Size = 44,
                };
            }

            return new FontImageSource
            {
                Glyph = FontAwesome.FontAwesomeIcons.Minus,
                FontFamily = "FontAwesomeSolid",
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
