// <copyright file="AwfulSearchPageRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Awful.Mobile;
using Awful.Mobile.Controls;
using Awful.Mobile.iOS;
using Awful.Mobile.Pages;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ForumListPage), typeof(AwfulSearchPageRenderer))]
[assembly: ExportRenderer(typeof(SAclopediaEntryListPage), typeof(AwfulSearchPageRenderer))]

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace Awful.Mobile.iOS
#pragma warning restore SA1300 // Element should begin with upper-case letter
{
    /// <summary>
    /// Awful Search Page Renderer.
    /// </summary>
    public class AwfulSearchPageRenderer : PageRenderer, IUISearchResultsUpdating
    {
        private readonly UISearchController searchController;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulSearchPageRenderer"/> class.
        /// </summary>
        public AwfulSearchPageRenderer()
        {
            this.searchController = new UISearchController(searchResultsController: null)
            {
                SearchResultsUpdater = this,
                DimsBackgroundDuringPresentation = false,
                HidesNavigationBarDuringPresentation = false,
                HidesBottomBarWhenPushed = true,
            };
            this.searchController.SearchBar.Placeholder = string.Empty;
        }

        /// <inheritdoc/>
        public override async void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (this.ParentViewController is UIViewController parentViewController)
            {
                if (parentViewController.NavigationItem.SearchController is null)
                {
                    this.ParentViewController.NavigationItem.SearchController = this.searchController;
                    this.DefinesPresentationContext = true;

                    // Work-around to ensure the SearchController appears when the page first appears https://stackoverflow.com/a/46313164/5953643
                    this.ParentViewController.NavigationItem.SearchController.Active = true;
                    this.ParentViewController.NavigationItem.SearchController.Active = false;
                }

                await this.UpdateBarButtonItems(parentViewController, (Page)this.Element).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (this.ParentViewController != null)
            {
                this.ParentViewController.NavigationItem.SearchController = null;
            }
        }

        /// <inheritdoc/>
        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            if (this.Element is IAwfulSearchPage searchPage)
            {
                searchPage.OnSearchBarTextChanged(this.searchController.SearchBar.Text ?? string.Empty);
            }
        }

        private static async Task<UIBarButtonItem> GetUIBarButtonItem(ToolbarItem toolbarItem)
        {
            var image = await GetUIImage(toolbarItem.IconImageSource).ConfigureAwait(false);

            if (image is null)
            {
                return new UIBarButtonItem(toolbarItem.Text, UIBarButtonItemStyle.Plain, (object sender, EventArgs e) => toolbarItem.Command?.Execute(toolbarItem.CommandParameter))
                {
                    AccessibilityIdentifier = toolbarItem.AutomationId,
                };
            }
            else
            {
                return new UIBarButtonItem(image, UIBarButtonItemStyle.Plain, (object sender, EventArgs e) => toolbarItem.Command?.Execute(toolbarItem.CommandParameter))
                {
                    AccessibilityIdentifier = toolbarItem.AutomationId,
                };
            }
        }

        private static Task<UIImage?> GetUIImage(ImageSource source) => source switch
        {
            FileImageSource _ => new FileImageSourceHandler().LoadImageAsync(source),
            UriImageSource _ => new ImageLoaderSourceHandler().LoadImageAsync(source),
            StreamImageSource _ => new StreamImagesourceHandler().LoadImageAsync(source),
            _ => Task.FromResult<UIImage?>(null)
        };

        private async Task UpdateBarButtonItems(UIViewController parentViewController, Page page)
        {
            var (leftBarButtonItem, rightBarButtonItems) = await this.GetToolbarItems(page.ToolbarItems).ConfigureAwait(false);

            if (leftBarButtonItem != null)
            {
                parentViewController.NavigationItem.LeftBarButtonItems = new UIBarButtonItem[] { leftBarButtonItem };
            }
            else
            {
                parentViewController.NavigationItem.LeftBarButtonItems = Array.Empty<UIBarButtonItem>();
            }

            if (rightBarButtonItems.Any())
            {
                parentViewController.NavigationItem.RightBarButtonItems = rightBarButtonItems.ToArray();
            }
            else
            {
                parentViewController.NavigationItem.RightBarButtonItems = Array.Empty<UIBarButtonItem>();
            }
        }

        private async Task<(UIBarButtonItem? LeftBarButtonItem, List<UIBarButtonItem> RightBarButtonItems)> GetToolbarItems(IEnumerable<ToolbarItem> items)
        {
            UIBarButtonItem? leftBarButtonItem = null;

            var leftToolbarItem = items.SingleOrDefault(x => x.Priority is 1);

            if (leftToolbarItem != null)
            {
                leftBarButtonItem = await GetUIBarButtonItem(leftToolbarItem).ConfigureAwait(false);
            }

            var rightBarButtonItems = new List<UIBarButtonItem>();

            foreach (var item in items.Where(x => x.Priority != 1))
            {
                var barButtonItem = await GetUIBarButtonItem(item).ConfigureAwait(false);
                rightBarButtonItems.Add(barButtonItem);
            }

            return (leftBarButtonItem, rightBarButtonItems);
        }
    }
}
