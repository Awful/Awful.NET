using Mazui.Tools.Web;
using Mazui.ViewModels;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mazui.Controls
{
    public sealed partial class PreviousPostsView : UserControl
    {
        public static PreviousPostsView Instance { get; set; }
        public PreviousPostsView()
        {
            this.InitializeComponent();
            ThreadFullView.NavigationCompleted += WebViewCommands.WebView_OnNavigationCompleted;
            ThreadFullView.ScriptNotify += WebViewCommands.WebViewNotifyCommand.WebView_ScriptNotify;
            Instance = this;
        }

        // strongly-typed view models enable x:bind
        public PreviousPostsViewModel ViewModel => this.DataContext as PreviousPostsViewModel;
    }
}
