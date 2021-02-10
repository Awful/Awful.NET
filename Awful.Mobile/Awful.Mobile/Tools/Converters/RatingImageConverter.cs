// <copyright file="RatingImageConverter.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Awful.Core.Entities.Threads;
using Awful.UI.Tools;
using Xamarin.Forms;

namespace Awful.Mobile.Tools.Converters
{
    /// <summary>
    /// Rating Image Converter.
    /// </summary>
    public class RatingImageConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ratingImage)
            {
                if (!string.IsNullOrEmpty(ratingImage))
                {
                    return ImageSource.FromResource($"Awful.UI.ThreadTags.{ratingImage}.png", typeof(AwfulAsyncCommand).GetTypeInfo().Assembly);
                }
            }

            return ImageSource.FromResource($"Awful.UI.ThreadTags.0stars.png", typeof(AwfulAsyncCommand).GetTypeInfo().Assembly);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
