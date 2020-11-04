// <copyright file="ThreadHeaderColorConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Awful.Windows.UI.Tools.Converters
{
    /// <summary>
    /// Convert Thread Header Color.
    /// </summary>
    public class ThreadHeaderColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            var item = (bool)value;
            if (item)
            {
                return Color.Yellow;
            }

            return Color.FromRgba(255, 65, 91, 100);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
