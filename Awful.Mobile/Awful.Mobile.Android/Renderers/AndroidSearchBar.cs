// <copyright file="AndroidSearchBar.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(Awful.Mobile.Droid.Renderers.AndroidSearchBar))]

namespace Awful.Mobile.Droid.Renderers
{
    /// <summary>
    /// Android Search Bar (No Icon)
    /// </summary>
    public class AndroidSearchBar : SearchBarRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidSearchBar"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public AndroidSearchBar(Context context)
            : base(context)
        {
        }

        /// <inheritdoc/>
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null)
            {
                var searchView = this.Control;
                searchView.Iconified = true;
                searchView.SetIconifiedByDefault(false);
                int searchIconId = this.Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var icon = searchView.FindViewById(searchIconId);
                icon.LayoutParameters = new LinearLayout.LayoutParams(0, 0);
            }
        }
    }
}