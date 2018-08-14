using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Awful.Tools.Converters
{
    public class ThreadHeaderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = (bool)value;
            if (item) return new SolidColorBrush(Colors.Yellow);
            return new SolidColorBrush(Color.FromArgb(255, 65, 91, 100));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BookmarksThreadHeaderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = (string)value;
            switch(item)
            {
                case "bm0":
                    return new SolidColorBrush(Colors.Orange);
                case "bm1":
                    return new SolidColorBrush(Colors.Red);
                case "bm2":
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.Orange);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
