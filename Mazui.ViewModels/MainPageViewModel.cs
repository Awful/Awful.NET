using Mazui.Core.Models.Forums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Mazui.Database.Functions;
using Mazui.Core.Tools;
using Windows.Storage;
using Newtonsoft.Json;
using Mazui.Tools.Authentication;
using Mazui.Core.Managers;
using Mazui.Core.Models.Users;
using Mazui.Tools;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Template10.Common;
using Mazui.Views;

namespace Mazui.ViewModels
{
    public class MainPageViewModel : MazuiViewModel
    {
        #region Properties
        public ObservableCollection<Category> ForumGroupList { get; set; } = new ObservableCollection<Category>();
        private Category _favoritesEntity;
        private WebManager _webManager;
        private ForumManager _forumManager;

        private UserAuth _user = Views.Shell.Instance.ViewModel.User;
        private bool _isLoggedIn = Views.Shell.Instance.ViewModel.IsLoggedIn;
        #endregion
        #region Methods

        public async Task Init()
        {
            try
            {
                if (!ForumGroupList.Any())
                {
                    IsLoading = true;
                    GetFavoriteForums();
                    await GetMainPageForumsAsync();
                    IsLoading = false;
                }
            }
            catch (Exception e)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to setup forum list: {e.Message}", false);
            }
        }

        public async Task AddOrRemoveFavorite(Forum forum)
        {
           try
            {
                var result = await ForumsDatabase.UpdateForumBookmark(forum);
                GetFavoriteForums();
            }
            catch (Exception e)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to add or remove favorite: {e.Message}", false);
            }
        }

        private async Task GetMainPageForumsAsync(bool forceRefresh = false)
        {
            var forumCategoryEntities = ForumsDatabase.GetMainForumCategories();
            if (forumCategoryEntities.Any() && !forceRefresh) { AddForumCategoryToPage(forumCategoryEntities); return; }
            if (!_isLoggedIn) { forumCategoryEntities = await LoadDefaultForums(); }
            if (_isLoggedIn && forceRefresh) forumCategoryEntities = await LoadForumsFromSite();
            ForumGroupList.Clear();
            foreach (var forumCategoryEntity in forumCategoryEntities) ForumGroupList.Add(forumCategoryEntity);
            RaisePropertyChanged("ForumGroupList");
            await ForumsDatabase.SaveForumList(ForumGroupList.ToList());
        }

        private async Task<List<Category>> LoadForumsFromSite()
        {
            if (_webManager == null) _webManager = await UserHandler.CreateAuthWebManager(_user);
            if (_forumManager == null) _forumManager = new ForumManager(_webManager);

            var forumResult = await _forumManager.GetForumCategoriesAsync();
            var resultCheck = await ResultChecker.CheckSuccess(forumResult);
            if (!resultCheck)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to update initial forum list", false);
                IsLoading = false;
                return new List<Category>();
            }
            return JsonConvert.DeserializeObject<List<Category>>(forumResult.ResultJson);
        }

        private async Task<List<Category>> LoadDefaultForums()
        {
            var sampleFile = @"Assets\Forums\forum.txt";
            var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await installationFolder.GetFileAsync(sampleFile);
            var sampleDataText = await FileIO.ReadTextAsync(file);
            return JsonConvert.DeserializeObject<List<Category>>(sampleDataText);
        }

        private void AddForumCategoryToPage(List<Category> forumCategoryEntities)
        {
            foreach (var forumCategoryEntity in forumCategoryEntities)
            {
                forumCategoryEntity.ForumList = forumCategoryEntity.ForumList.OrderBy(node => node.Order).ToList();
                ForumGroupList.Add(forumCategoryEntity);
            }
        }

        private void GetFavoriteForums()
        {
            var forumEntities = ForumsDatabase.GetFavoriteForums();
            var favorites = RemoveCurrentFavoritesFromList(forumEntities);
            _favoritesEntity = new Category
            {
                Name = "Favorites",
                Location = string.Format(EndPoints.ForumPage, "forumid=48"),
                ForumList = forumEntities
            };
            if (favorites == null)
            {
                if (forumEntities.Any()) ForumGroupList.Insert(0, _favoritesEntity);
            }
            else
            {
                if (ForumGroupList.First().Name == _favoritesEntity.Name) ForumGroupList.RemoveAt(0);
                if (forumEntities.Any()) ForumGroupList.Insert(0, _favoritesEntity);
            }
        }

        private Category RemoveCurrentFavoritesFromList(List<Forum> forumEntities)
        {
            var favorites = ForumGroupList.FirstOrDefault(node => node.Name.Equals("Favorites"));
            if (!forumEntities.Any())
            {
                if (favorites != null)
                {
                    ForumGroupList.Remove(favorites);
                }
            }
            return favorites;
        }
        #endregion
    }
}
