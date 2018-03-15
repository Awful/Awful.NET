using Awful.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Awful.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPageViewModel ViewModel => this.DataContext as LoginPageViewModel;

        public LoginPage()
        {
            this.InitializeComponent();
            ViewModel.Load();
            App.ShellViewModel.Header = "Profile";
        }
    }
}
