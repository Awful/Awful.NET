using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Awful.Database.Functions;
using Awful.Helpers;
using Awful.Parser.Managers;
using Awful.Parser.Models.Forums;
using Awful.Services;
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

        public async Task LoadAsync(bool forceRefresh = false)
        {
            IsLoading = true;
            try
            {
                LoginUser();
                GetFavoriteForums();
                await GetMainPageForumsAsync(forceRefresh);
            }
            catch (Exception e)
            {
                await ResultChecker.SendMessageDialogAsync($"Failed to setup forum list: {e.Message}", false);
            }
            IsLoading = false;
        }

        public void NavigateToThreadList(Forum forum)
        {
            NavigationService.Navigate(typeof(Views.ThreadListPage), JsonConvert.SerializeObject(forum));
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
                Location = string.Format(Awful.Parser.Core.EndPoints.ForumPage, "forumid=48"),
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
            if (forumCategoryEntities.Any() && !forceRefresh)
            {
                AddForumCategoryToPage(forumCategoryEntities);
                return;
            }
            else
                forumCategoryEntities = await LoadForumsFromSite();
            ForumGroupList.Clear();
            foreach (var forumCategoryEntity in forumCategoryEntities) ForumGroupList.Add(forumCategoryEntity);
            OnPropertyChanged("ForumGroupList");
            await ForumsDatabase.SaveForumList(ForumGroupList.ToList());
        }

        private async Task<List<Category>> LoadForumsFromSite()
        {
            if (_forumManager == null) _forumManager = new ForumManager(WebManager);

            try
            {
                var forumResult = await _forumManager.GetForumCategoriesAsync();
                IsLoading = false;
                return forumResult;
            }
            catch (Exception ex)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to update initial forum list", false);
                IsLoading = false;
                return new List<Category>();
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
    }
}
