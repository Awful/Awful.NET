using AmazingPullToRefresh.Controls;
using Mazui.Core.Models.Forums;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Mazui.XboxViews
{
	public sealed partial class MainPage : Page
	{
		public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

		public MainPage()
		{
			this.InitializeComponent();
		}

		private void MainForumListFull_OnItemClick(object sender, ItemClickEventArgs e)
		{
			var forum = e.ClickedItem as Forum;
			//ViewModel.NavigateToThreadList(forum);
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			await ViewModel.Init();
			ForumViewSource.Source = ViewModel.ForumGroupList;
			var collectionGroups = ForumViewSource.View.CollectionGroups;
			((ListViewBase)this.semanticZoom.ZoomedOutView).ItemsSource = collectionGroups;
		}

		private async void AddOrRemoveFavorite(object sender, RoutedEventArgs e)
		{
			var menuFlyoutItem = sender as MenuFlyoutItem;
			var forum = menuFlyoutItem?.CommandParameter as Forum;
			if (forum != null)
			{
				await ViewModel.AddOrRemoveFavorite(forum);
			}
		}

		private async void PullToRefreshExtender_RefreshRequested(object sender, RefreshRequestedEventArgs e)
		{
			await ViewModel.RefreshForums();
		}

		private void MenuList_ItemClick(object sender, ItemClickEventArgs e)
		{
			var menuItem = e.ClickedItem as string;
			if (string.IsNullOrEmpty(menuItem)) return;
			switch(menuItem)
			{
				case "Account":
					ViewModel.NavigateToXboxLogin();
					break;
				case "Bookmarks":
					if (!ViewModel.IsLoggedIn)
					{
						ViewModel.NavigateToXboxLogin();
					}
					break;
				case "Private Messages":
					if (!ViewModel.IsLoggedIn)
					{
						ViewModel.NavigateToXboxLogin();
					}
					break;
				case "Settings":

					break;
				default:
					return;
			}
		}
	}
}
