using Awful.Models.Forums;
using System;
using Windows.UI.Xaml.Data;

namespace Awful.Tools.Converters
{
    public class AddOrRemoveFavoriteTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var forum = (Forum)value;
            return forum.IsBookmarks ? "Remove Favorite" : "Add as Favorite";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
