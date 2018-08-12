using Awful.Parser.Models.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Awful.Tools.Converters
{
    public class ForwardButtonEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentThread = value as Thread;
            if (currentThread == null) return false;
            return currentThread.TotalPages != currentThread.CurrentPage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
