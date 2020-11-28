// <copyright file="LastPageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Awful.Core.Entities.Threads;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Last Page Converter.
    /// </summary>
    public class LastPageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ThreadPost thread)
            {
                return thread.CurrentPage < thread.TotalPages;
            }

            return false;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
