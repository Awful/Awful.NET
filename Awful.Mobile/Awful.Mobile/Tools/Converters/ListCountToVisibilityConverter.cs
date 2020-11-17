// <copyright file="ListCountToVisibilityConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Awful.Mobile.UI.Tools.Converters
{
    public class ListCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list)
            {
                return list.Count > 0;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
