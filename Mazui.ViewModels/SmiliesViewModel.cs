using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Template10.Mvvm;
using Template10.Utils;
using Mazui.Core.Models.Smilies;
using Mazui.Core.Managers;
using Mazui.Controls;

namespace Mazui.ViewModels
{
    public class SmiliesViewModel : MazuiViewModel
    {
        public SmiliesView SmiliesView { get; set; }

        private ObservableCollection<SmileCategory> _smileCategoryList = new ObservableCollection<SmileCategory>();

        public ObservableCollection<SmileCategory> SmileCategoryList
        {
            get { return _smileCategoryList; }
            set
            {
                Set(ref _smileCategoryList, value);
            }
        }

        public TextBox ReplyBox { get; set; }

        private bool _isOpen = default(bool);

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                Set(ref _isOpen, value);
            }
        }

        private SmileManager _smileManager;
        public ObservableCollection<SmileCategory> FullSmileCategoryEntities { get; set; }


        public async Task LoadSmilies()
        {
            if (WebManager == null)
            {
                await LoginUser();
                _smileManager = new SmileManager(WebManager);
            }

            if (!SmileCategoryList.Any())
            {
                IsLoading = true;
                var result = await _smileManager.GetSmileList();
                FullSmileCategoryEntities = result.ToObservableCollection();
                foreach (var item in result)
                {
                    SmileCategoryList.Add(item);
                }
                IsLoading = false;
            }

            SmiliesView.Init();
        }

        public void SelectIcon(object sender, ItemClickEventArgs e)
        {
            var smile = e.ClickedItem as Smile;
            if (smile == null) return;
            ReplyBox.Text = ReplyBox.Text.Insert(ReplyBox.Text.Length, smile.Title);
            IsOpen = false;
        }

        public void SmiliesFilterOnSuggestedQuery(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var suggestionCollection = args.Request.SearchSuggestionCollection;
            foreach (var smile in SmileCategoryList.SelectMany(smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText))))
            {
                suggestionCollection.AppendQuerySuggestion(smile.Title);
            }
        }

        public void SmiliesFilterOnSubmittedQuery(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var result = SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result == null)
            {
                return;
            }
            ReplyBox.Text = ReplyBox.Text.Insert(ReplyBox.Text.Length, result.Title);
            IsOpen = false;
        }

        public void SmiliesFilterOnChangedQuery(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText))
            {
                SmileCategoryList = FullSmileCategoryEntities;
                return;
            }
            var result = SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result != null) return;
            var searchList = FullSmileCategoryEntities.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText)));
            List<Smile> smileListEntities = searchList.ToList();
            var searchSmileCategory = new SmileCategory()
            {
                Name = "Search",
                SmileList = smileListEntities
            };
            var fakeSmileCategoryList = new List<SmileCategory> { searchSmileCategory };
            SmileCategoryList = fakeSmileCategoryList.ToObservableCollection();
        }
    }
}
