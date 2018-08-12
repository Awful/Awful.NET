using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Posts;
using Awful.Parser.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using PropertyChanged;
using System.ComponentModel;

namespace Awful.Parser.Models.Threads
{
    public class Thread : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public string Location { get; set; }

        public string ImageIconUrl { get; set; } = "";

        public string ImageIconLocation { get; set; }

        public string StoreImageIconUrl { get; set; }

        public string StoreImageIconLocation { get; set; }

        public string Author { get; set; }

        public long AuthorId { get; set; }

        public int ReplyCount { get; set; }

        public int ViewCount { get; set; }

        public decimal Rating { get; set; }

        public int TotalRatingVotes { get; set; }

        public string RatingImage { get; set; }

        public string RatingImageUrl { get; set; }

        public string KilledBy { get; set; }

        public long KilledById { get; set; }

        public DateTime KilledOn { get; set; }

        public bool IsArchived { get; set; }

        public bool IsSticky { get; set; }

        public bool IsNotified { get; set; }

        public bool IsLocked { get; set; }

        [NotMapped]
        public bool IsLoggedIn { get; set; }

        public bool IsAnnouncement { get; set; }

        public bool HasBeenViewed { get; set; }

        public bool CanMarkAsUnread { get; set; }

        public int RepliesSinceLastOpened { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int ScrollToPost { get; set; }

        public string ScrollToPostString { get; set; }

        public string LoggedInUserName { get; set; }

        [Key]
        public int ThreadId { get; set; }

        public int ForumId { get; set; }

        public Forum ForumEntity { get; set; }

        public bool HasSeen { get; set; }

        public bool IsBookmark { get; set; }

        public string StarColor { get; set; }

        public string Html { get; set; }

        public bool IsPrivateMessage { get; set; }

        public int OrderNumber { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public Thread Clone()
        {
            return MemberwiseClone() as Thread;
        }
    }
}
