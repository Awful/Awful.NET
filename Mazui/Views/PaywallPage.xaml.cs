using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Services.NavigationService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Mazui.Views
{
    public sealed partial class PaywallPage : Page
    {
        public PaywallPage()
        {
            this.InitializeComponent();
            Template10.Common.BootStrapper.Current.NavigationService.ClearHistory();
            var length = Template10.Common.BootStrapper.Current.NavigationService.Frame.BackStack.Count;
            Template10.Common.BootStrapper.Current.NavigationService.Frame.BackStack.RemoveAt(length - 1);
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://secure.somethingawful.com/products/register.php"));
        }
    }
}
