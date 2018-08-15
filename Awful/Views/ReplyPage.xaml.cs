using Awful.Parser.Models.Threads;
using Awful.Services;
using Awful.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Awful.Views
{
    public sealed partial class ReplyPage : Page
    {
        public ReplyThreadViewModel ViewModel => this.DataContext as ReplyThreadViewModel;

        public ReplyPage()
        {
            this.InitializeComponent();
            SmiliesView.ViewModel.ReplyBox = ReplyText;
            PreviousView.ViewModel.ReplyBox = ReplyText;
            ViewModel.PreviousPostsViewModel = PreviousView.ViewModel;
            ViewModel.PreviewViewModel = PreviewView.ViewModel;
            ViewModel.SmiliesViewModel = SmiliesView.ViewModel;
            ViewModel.ReplyBox = ReplyText;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) return;
            await ViewModel.Init(JsonConvert.DeserializeObject<ThreadReply>(e.Parameter as string));

        }
    }
}
