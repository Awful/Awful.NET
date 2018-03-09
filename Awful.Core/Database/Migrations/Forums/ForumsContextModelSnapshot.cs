using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Awful.Database.Context;

namespace Awful.Migrations.Forums
{
    [DbContext(typeof(ForumsContext))]
    partial class ForumsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Awful.Models.Forums.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Awful.Models.Forums.Forum", b =>
                {
                    b.Property<int>("ForumId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<int>("CurrentPage");

                    b.Property<string>("Description");

                    b.Property<bool>("IsBookmarks");

                    b.Property<bool>("IsSubforum");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<int>("TotalPages");

                    b.HasKey("ForumId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("Awful.Models.Posts.Post", b =>
                {
                    b.Property<long>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasSeen");

                    b.Property<bool>("IsQuoting");

                    b.Property<string>("PostDate");

                    b.Property<string>("PostDivString");

                    b.Property<string>("PostFormatted");

                    b.Property<string>("PostHtml");

                    b.Property<long>("PostIndex");

                    b.Property<string>("PostLink");

                    b.Property<string>("PostMarkdown");

                    b.Property<string>("PostQuoteLink");

                    b.Property<string>("PostReportLink");

                    b.Property<int?>("ThreadId");

                    b.Property<long?>("UserId");

                    b.HasKey("PostId");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Awful.Models.Threads.Thread", b =>
                {
                    b.Property<int>("ThreadId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<bool>("CanMarkAsUnread");

                    b.Property<int>("CurrentPage");

                    b.Property<int>("ForumId");

                    b.Property<bool>("HasBeenViewed");

                    b.Property<bool>("HasSeen");

                    b.Property<string>("Html");

                    b.Property<string>("ImageIconLocation");

                    b.Property<string>("ImageIconUrl");

                    b.Property<bool>("IsAnnouncement");

                    b.Property<bool>("IsBookmark");

                    b.Property<bool>("IsLocked");

                    b.Property<bool>("IsNotified");

                    b.Property<bool>("IsPrivateMessage");

                    b.Property<bool>("IsSticky");

                    b.Property<string>("KilledBy");

                    b.Property<string>("Location");

                    b.Property<string>("LoggedInUserName");

                    b.Property<string>("Name");

                    b.Property<int>("OrderNumber");

                    b.Property<int>("Rating");

                    b.Property<string>("RatingImage");

                    b.Property<string>("RatingImageUrl");

                    b.Property<int>("RepliesSinceLastOpened");

                    b.Property<int>("ReplyCount");

                    b.Property<int>("ScrollToPost");

                    b.Property<string>("ScrollToPostString");

                    b.Property<string>("StoreImageIconLocation");

                    b.Property<string>("StoreImageIconUrl");

                    b.Property<int>("TotalPages");

                    b.Property<int>("ViewCount");

                    b.HasKey("ThreadId");

                    b.HasIndex("ForumId");

                    b.ToTable("BookmarkedThreads");
                });

            modelBuilder.Entity("Awful.Models.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutUser");

                    b.Property<string>("AimContactString");

                    b.Property<string>("AvatarHtml");

                    b.Property<string>("AvatarLink");

                    b.Property<string>("AvatarTitle");

                    b.Property<bool>("CanSendPrivateMessage");

                    b.Property<DateTime>("DateJoined");

                    b.Property<string>("HomePageString");

                    b.Property<string>("IcqContactString");

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsCurrentUserPost");

                    b.Property<bool>("IsMod");

                    b.Property<DateTime>("LastPostDate");

                    b.Property<string>("Location");

                    b.Property<int>("PostCount");

                    b.Property<string>("PostHistoryLink");

                    b.Property<string>("PostRate");

                    b.Property<string>("PrivateMessageLink");

                    b.Property<string>("ProfileLink");

                    b.Property<string>("RapSheetLink");

                    b.Property<string>("Roles");

                    b.Property<string>("SellerRating");

                    b.Property<string>("Username");

                    b.Property<string>("YahooContactString");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Awful.Models.Forums.Forum", b =>
                {
                    b.HasOne("Awful.Models.Forums.Category", "Category")
                        .WithMany("ForumList")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ForeignKey_Forum_Category")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Awful.Models.Posts.Post", b =>
                {
                    b.HasOne("Awful.Models.Threads.Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadId");

                    b.HasOne("Awful.Models.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Awful.Models.Threads.Thread", b =>
                {
                    b.HasOne("Awful.Models.Forums.Forum", "ForumEntity")
                        .WithMany()
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
