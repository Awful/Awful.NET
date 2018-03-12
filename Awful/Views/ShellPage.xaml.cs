using System;

using Awful.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Awful.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
            App.ShellViewModel = ViewModel;
        }
    }
}
