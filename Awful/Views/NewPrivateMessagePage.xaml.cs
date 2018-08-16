using Awful.Parser.Models.Messages;
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
    public sealed partial class NewPrivateMessagePage : Page
    {
        public NewPrivateMessageViewModel ViewModel => this.DataContext as NewPrivateMessageViewModel;

        public NewPrivateMessagePage()
        {
            this.InitializeComponent();
            SmiliesView.ViewModel.ReplyBox = ReplyText;
            ViewModel.Subject = Subject;
            ViewModel.PostIconViewModel = PostIconView.ViewModel;
            ViewModel.SmiliesViewModel = SmiliesView.ViewModel;
            ViewModel.ReplyBox = ReplyText;
            ViewModel.Recipient = Recipient;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) return;
            await ViewModel.Init(JsonConvert.DeserializeObject<PrivateMessage>(e.Parameter as string));
        }
    }
}
