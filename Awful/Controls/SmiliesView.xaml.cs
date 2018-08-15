﻿using Awful.ViewModels;
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

namespace Awful.Controls
{
    public sealed partial class SmiliesView : UserControl
    {
        public SmiliesView()
        {
            this.InitializeComponent();
            ViewModel.LoginUser();
            ViewModel.SmiliesView = this;
        }

        public void Init()
        {
            ForumViewSource.Source = ViewModel.SmileCategoryList;
            var collectionGroups = ForumViewSource.View.CollectionGroups;
            ((ListViewBase)this.semanticZoom.ZoomedOutView).ItemsSource = collectionGroups;
        }

        // strongly-typed view models enable x:bind
        public SmiliesViewModel ViewModel => this.DataContext as SmiliesViewModel;
    }
}
