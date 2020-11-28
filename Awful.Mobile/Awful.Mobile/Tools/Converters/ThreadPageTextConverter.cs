// <copyright file="ThreadPageTextConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Awful.Core.Entities.Threads;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Thread Page Text Converter.
    /// </summary>
    public class ThreadPageTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ThreadPost threadPosts)
            {
                return $"{threadPosts.CurrentPage} / {threadPosts.TotalPages}";
            }

            return "0 / 0";
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
