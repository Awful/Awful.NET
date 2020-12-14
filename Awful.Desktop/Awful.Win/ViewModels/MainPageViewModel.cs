// <copyright file="MainPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awful.Database.Context;
using Awful.UI.Interfaces;
using Awful.UI.Tools;
using Awful.UI.ViewModels;
using Awful.Win.Controls;
using Awful.Win.Pages;
using Microsoft.UI.Xaml.Controls;

namespace Awful.Win.ViewModels
{
    /// <summary>
    /// Main Page View Model.
    /// </summary>
    public class MainPageViewModel : AwfulViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Awful Navigation handler.</param>
        /// <param name="error">Awful Error handler.</param>
        /// <param name="context">Awful Context.</param>
        public MainPageViewModel(IAwfulNavigation navigation, IAwfulErrorHandler error, IAwfulContext context)
            : base(navigation, error, context)
        {
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public ObservableCollection<AwfulMenuCategory> Categories
        {
            get
            {
                return new ObservableCollection<AwfulMenuCategory>()
                {
                    new AwfulMenuCategory()
                    {
                        Page = typeof(DebugPage),
                        Name = "Debug",
                        CategoryIcon = "Add",
                    },
                };
            }
        }

        /// <summary>
        /// Gets the item selection command.
        /// </summary>
        public AwfulAsyncCommand<string> ItemSelectionCommand
        {
            get
            {
                return new AwfulAsyncCommand<string>(
                    (menuString) =>
                    {
                        if (menuString == "Settings")
                        {
                            this.Navigation.PushPageAsync(typeof(SettingsPage));
                            return Task.CompletedTask;
                        }

                        var menuItem = this.Categories.FirstOrDefault(n => n.Name == menuString);
                        if (menuItem == null)
                        {
                            return Task.CompletedTask;
                        }

                        this.Navigation.PushPageAsync(menuItem.Page);

                        return Task.CompletedTask;
                    },
                    null,
                    this.Error);
            }
        }

        /// <summary>
        /// Setup Theme.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SetupThemeAsync()
        {
            await this.Navigation.SetupThemeAsync().ConfigureAwait(false);
        }
    }
}
