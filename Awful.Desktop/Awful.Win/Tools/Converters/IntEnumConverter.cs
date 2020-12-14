// <copyright file="IntEnumConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Webview.Entities.Themes;
using Microsoft.UI.Xaml.Data;

namespace Awful.Win.Tools.Converters
{
    /// <summary>
    /// Convert int to enum.
    /// </summary>
    public class IntEnumConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum)
            {
                return (int)value;
            }

            return 0;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
            {
                return Enum.ToObject(typeof(DeviceColorTheme), value);
            }

            return 0;
        }
    }
}
