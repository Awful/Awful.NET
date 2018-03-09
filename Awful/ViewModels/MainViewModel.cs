using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Managers;
using Awful.Models.Forums;
using Awful.Tools;
using Newtonsoft.Json;
using Windows.Storage;

namespace Awful.ViewModels
{
    public class MainViewModel : AwfulViewModel
    {
        #region Properties
        public ObservableCollection<Category> ForumGroupList { get; set; } = new ObservableCollection<Category>();
        private Category _favoritesEntity;
        private ForumManager _forumManager;
        #endregion

        public MainViewModel()
        {
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

        public async Task LoadAsync()
        {
            try
            {
                if (WebManager == null)
                {
                    LoginUser();
                }

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

        private async Task GetMainPageForumsAsync(bool forceRefresh = false)
        {
            var forumCategoryEntities = ForumsDatabase.GetMainForumCategories();
            if (forumCategoryEntities.Any() && !forceRefresh) { AddForumCategoryToPage(forumCategoryEntities); return; }
            if (!IsLoggedIn) { forumCategoryEntities = await LoadDefaultForums(); }
            if (IsLoggedIn && forceRefresh) forumCategoryEntities = await LoadForumsFromSite();
            ForumGroupList.Clear();
            foreach (var forumCategoryEntity in forumCategoryEntities) ForumGroupList.Add(forumCategoryEntity);
            //RaisePropertyChanged("ForumGroupList");
            await ForumsDatabase.SaveForumList(ForumGroupList.ToList());
        }

        private async Task<List<Category>> LoadDefaultForums()
        {
            var sampleFile = @"Assets\Forums\forums.json";
            var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await installationFolder.GetFileAsync(sampleFile);
            var sampleDataText = await FileIO.ReadTextAsync(file);
            return JsonConvert.DeserializeObject<List<Category>>(sampleDataText);
        }

        private async Task<List<Category>> LoadForumsFromSite()
        {
            if (_forumManager == null) _forumManager = new ForumManager(WebManager);

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
    }
}
