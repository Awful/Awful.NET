// <copyright file="BookmarksThreadHeaderColorConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Core.Entities.Threads;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Bookmark Thread Header Color Converter.
    /// </summary>
    public class BookmarksThreadHeaderColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Thread awfulThread)
            {
                if (awfulThread.IsSticky)
                {
                    return Color.Yellow;
                }

                switch (awfulThread.StarColor)
                {
                    case "bm0":
                        return Color.Orange;
                    case "bm1":
                        return Color.Red;
                    case "bm2":
                        return Color.Yellow;
                    case "bm3":
                        return Color.Blue;
                    case "bm4":
                        return Color.Green;
                    case "bm5":
                        return Color.Purple;
                    default:
                        return Color.FromHex("#1483B1");
                }
            }

            return Color.FromHex("#1483B1");
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
