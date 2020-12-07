// <copyright file="AwfulSearchPageRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Views.InputMethods;
using AndroidX.AppCompat.Widget;
using Awful.Mobile.Controls;
using Awful.Mobile.Droid;
using Awful.Mobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//TODO: Fix for Android
//[assembly: ExportRenderer(typeof(ForumListPage), typeof(AwfulSearchPageRenderer))]
//[assembly: ExportRenderer(typeof(SAclopediaEntryListPage), typeof(AwfulSearchPageRenderer))]

namespace Awful.Mobile.Droid
{
    /// <summary>
    /// Awful Search Page Renderer.
    /// </summary>
    public class AwfulSearchPageRenderer : PageRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulSearchPageRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public AwfulSearchPageRenderer(Context context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            if (this.Element is IAwfulSearchPage && this.Element is Page page && page.Parent is NavigationPage navigationPage)
            {
                navigationPage.Popped += this.HandleNavigationPagePopped;
                navigationPage.PoppedToRoot += this.HandleNavigationPagePopped;
            }
        }

        void HandleNavigationPagePopped(object sender, NavigationEventArgs e)
        {
            if (sender is NavigationPage navigationPage
                && navigationPage.CurrentPage is IAwfulSearchPage)
            {
                this.AddSearchToToolbar(navigationPage.CurrentPage.Title);
            }
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (this.Element is IAwfulSearchPage && this.Element is Page page && page.Parent is NavigationPage navigationPage && navigationPage.CurrentPage is IAwfulSearchPage)
            {
                this.AddSearchToToolbar(page.Title);
            }
        }

        /// <inheritdoc/>
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            System.Diagnostics.Debug.WriteLine(e.OldElement);
            System.Diagnostics.Debug.WriteLine(e.NewElement);
            if (e.NewElement is IAwfulSearchPage && e.NewElement is Page page && page.Parent is NavigationPage navigationPage && navigationPage.CurrentPage is IAwfulSearchPage)
            {
                this.AddSearchToToolbar(page.Title);
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (this.GetToolbar() is Toolbar toolBar)
            {
                toolBar.Menu?.RemoveItem(Resource.Menu.MainMenu);
            }

            base.Dispose(disposing);
        }

        void AddSearchToToolbar(in string pageTitle)
        {
            if (this.GetToolbar() is Toolbar toolBar
                && toolBar.Menu?.FindItem(Resource.Id.ActionSearch)?.ActionView?.JavaCast<SearchView>()?.GetType() != typeof(SearchView))
            {
                toolBar.Title = pageTitle;
                toolBar.InflateMenu(Resource.Menu.MainMenu);

                if (toolBar.Menu?.FindItem(Resource.Id.ActionSearch)?.ActionView?.JavaCast<SearchView>() is SearchView searchView)
                {
                    searchView.QueryTextChange += this.HandleQueryTextChange;
                    searchView.ImeOptions = (int)ImeAction.Search;
                    searchView.InputType = (int)InputTypes.TextVariationFilter;
                    searchView.MaxWidth = int.MaxValue;
                    searchView.Elevation = this.Resources?.GetDimension(Resource.Dimension.toolbar_elevation) ?? 6;
                }
            }
        }

        void HandleQueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (this.Element is IAwfulSearchPage searchPage)
            {
                searchPage.OnSearchBarTextChanged(e.NewText);
            }
        }

        Toolbar? GetToolbar()
        {
            var activity = this.Context as Android.App.Activity;
            if (activity == null)
            {
                return null;
            }

            return activity.FindViewById<Toolbar>(Resource.Id.toolbar);
        }
    }
}