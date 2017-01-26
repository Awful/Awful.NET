using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Mazui.Tools.Threads
{
    public static class WebViewHelper
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(WebViewHelper), new PropertyMetadata(null, OnHtmlChanged));

        public static readonly DependencyProperty HtmlUriProperty = DependencyProperty.RegisterAttached("HtmlUri",
            typeof(Uri),
            typeof(
                WebViewHelper
                ),
            new PropertyMetadata
                (null,
                    OnHtmlUriChanged));

        public static string GetHtml(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebView;
            if (browser == null)
                return;
            string html = string.Empty;
            if (e.NewValue != null)
            {
                html = e.NewValue.ToString();
            }
            browser.NavigateToString(html);
        }

        public static void SetHtmlUri(DependencyObject dependencyObject, Uri value)
        {
            dependencyObject.SetValue(HtmlUriProperty, value);
        }

        public static Uri GetHtmlUri(DependencyObject dependencyObject)
        {
            return (Uri)dependencyObject.GetValue(HtmlUriProperty);
        }

        private static void OnHtmlUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebView;
            if (browser == null)
                return;

            var htmlUri = (Uri)e.NewValue;
            browser.Navigate(htmlUri);
        }

        public static async Task<bool> HtmlExists(string htmlFile)
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(htmlFile);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }
}
