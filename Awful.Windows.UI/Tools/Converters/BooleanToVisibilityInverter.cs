// <copyright file="BooleanToVisibilityInverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Boolean to Visibility Inverter.
    /// </summary>
    public class BooleanToVisibilityInverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value == false) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
