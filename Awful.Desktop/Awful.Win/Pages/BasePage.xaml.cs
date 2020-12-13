// <copyright file="BasePage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Awful.Win.Pages
{
    /// <summary>
    /// Base Awful Page.
    /// </summary>
    public partial class BasePage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class.
        /// Base Awful Page.
        /// </summary>
        public BasePage()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.DataContext is AwfulViewModel vm)
            {
                vm.OnLoadCommand.ExecuteAsync().FireAndForgetSafeAsync(vm.Error);
            }
        }
    }
}
