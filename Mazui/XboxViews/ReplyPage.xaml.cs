using Mazui.Controls;
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

namespace Mazui.XboxViews
{
	public sealed partial class ReplyPage : Page
	{
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

		protected override async void OnNavigatedFrom(NavigationEventArgs e)
		{
			if (e.Content is BookmarkPage)
			{
				var bookmarkPage = e.Content as BookmarkPage;
				await bookmarkPage.ViewModel.XboxThreadView.ViewModel.LoadThread();
			}
			else if (e.Content is ThreadListPage)
			{
				var threadListPage = e.Content as ThreadListPage;
				await threadListPage.ViewModel.XboxThreadView.ViewModel.LoadThread();
			}
		}

		// strongly-typed view models enable x:bind
		public ReplyThreadViewModel ViewModel => this.DataContext as ReplyThreadViewModel;
	}
}
