using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awful.Core.Migrations
{
    public partial class FixConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: true),
                    AvatarLink = table.Column<string>(nullable: true),
                    UserPicLink = table.Column<string>(nullable: true),
                    AvatarTitle = table.Column<string>(nullable: true),
                    AvatarHtml = table.Column<string>(nullable: true),
                    AvatarGangTagLink = table.Column<string>(nullable: true),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    ProfileLink = table.Column<string>(nullable: true),
                    PrivateMessageLink = table.Column<string>(nullable: true),
                    PostHistoryLink = table.Column<string>(nullable: true),
                    RapSheetLink = table.Column<string>(nullable: true),
                    CanSendPrivateMessage = table.Column<bool>(nullable: false),
                    IcqContactString = table.Column<string>(nullable: true),
                    AimContactString = table.Column<string>(nullable: true),
                    YahooContactString = table.Column<string>(nullable: true),
                    HomePageString = table.Column<string>(nullable: true),
                    PostCount = table.Column<int>(nullable: false),
                    LastPostDate = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    AboutUser = table.Column<string>(nullable: true),
                    IsMod = table.Column<bool>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Roles = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    IsCurrentUserPost = table.Column<bool>(nullable: false),
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostRate = table.Column<string>(nullable: true),
                    SellerRating = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CurrentPage = table.Column<int>(nullable: false),
                    IsSubForum = table.Column<bool>(nullable: false),
                    TotalPages = table.Column<int>(nullable: false),
                    ForumId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentForumId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    IsBookmarks = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    TotalTopics = table.Column<int>(nullable: false),
                    TotalPosts = table.Column<int>(nullable: false),
                    ForumId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.ForumId);
                    table.ForeignKey(
                        name: "ForeignKey_Forum_Category",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forums_Forums_ForumId1",
                        column: x => x.ForumId1,
                        principalTable: "Forums",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Forums_Forums_ParentForumId",
                        column: x => x.ParentForumId,
                        principalTable: "Forums",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookmarkedThreads",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    ImageIconUrl = table.Column<string>(nullable: true),
                    ImageIconLocation = table.Column<string>(nullable: true),
                    StoreImageIconUrl = table.Column<string>(nullable: true),
                    StoreImageIconLocation = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    AuthorId = table.Column<long>(nullable: false),
                    ReplyCount = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    Rating = table.Column<decimal>(nullable: false),
                    TotalRatingVotes = table.Column<int>(nullable: false),
                    RatingImage = table.Column<string>(nullable: true),
                    RatingImageUrl = table.Column<string>(nullable: true),
                    KilledBy = table.Column<string>(nullable: true),
                    KilledById = table.Column<long>(nullable: false),
                    KilledOn = table.Column<DateTime>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    IsSticky = table.Column<bool>(nullable: false),
                    IsNotified = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    IsAnnouncement = table.Column<bool>(nullable: false),
                    HasBeenViewed = table.Column<bool>(nullable: false),
                    CanMarkAsUnread = table.Column<bool>(nullable: false),
                    RepliesSinceLastOpened = table.Column<int>(nullable: false),
                    TotalPages = table.Column<int>(nullable: false),
                    CurrentPage = table.Column<int>(nullable: false),
                    ScrollToPost = table.Column<int>(nullable: false),
                    ScrollToPostString = table.Column<string>(nullable: true),
                    LoggedInUserName = table.Column<string>(nullable: true),
                    ThreadId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForumId = table.Column<int>(nullable: false),
                    HasSeen = table.Column<bool>(nullable: false),
                    IsBookmark = table.Column<bool>(nullable: false),
                    StarColor = table.Column<string>(nullable: true),
                    Html = table.Column<string>(nullable: true),
                    IsPrivateMessage = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookmarkedThreads", x => x.ThreadId);
                    table.ForeignKey(
                        name: "FK_BookmarkedThreads_Forums_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forums",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: true),
                    PostDate = table.Column<string>(nullable: true),
                    PostReportLink = table.Column<string>(nullable: true),
                    PostQuoteLink = table.Column<string>(nullable: true),
                    PostLink = table.Column<string>(nullable: true),
                    PostFormatted = table.Column<string>(nullable: true),
                    PostHtml = table.Column<string>(nullable: true),
                    PostMarkdown = table.Column<string>(nullable: true),
                    PostId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostIndex = table.Column<long>(nullable: false),
                    PostDivString = table.Column<string>(nullable: true),
                    HasSeen = table.Column<bool>(nullable: false),
                    IsQuoting = table.Column<bool>(nullable: false),
                    ThreadId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_BookmarkedThreads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "BookmarkedThreads",
                        principalColumn: "ThreadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkedThreads_ForumId",
                table: "BookmarkedThreads",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_CategoryId",
                table: "Forums",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_ForumId1",
                table: "Forums",
                column: "ForumId1");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_ParentForumId",
                table: "Forums",
                column: "ParentForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ThreadId",
                table: "Post",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "BookmarkedThreads");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
