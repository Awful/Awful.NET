using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awful.Migrations.Forums
{
    public partial class Bookmarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookmarkedThreads",
                columns: table => new
                {
                    ThreadId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Author = table.Column<string>(nullable: true),
                    CanMarkAsUnread = table.Column<bool>(nullable: false),
                    CurrentPage = table.Column<int>(nullable: false),
                    ForumId = table.Column<int>(nullable: false),
                    HasBeenViewed = table.Column<bool>(nullable: false),
                    HasSeen = table.Column<bool>(nullable: false),
                    Html = table.Column<string>(nullable: true),
                    ImageIconLocation = table.Column<string>(nullable: true),
                    ImageIconUrl = table.Column<string>(nullable: true),
                    IsAnnouncement = table.Column<bool>(nullable: false),
                    IsBookmark = table.Column<bool>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false),
                    IsNotified = table.Column<bool>(nullable: false),
                    IsPrivateMessage = table.Column<bool>(nullable: false),
                    IsSticky = table.Column<bool>(nullable: false),
                    KilledBy = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    LoggedInUserName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    RatingImage = table.Column<string>(nullable: true),
                    RatingImageUrl = table.Column<string>(nullable: true),
                    RepliesSinceLastOpened = table.Column<int>(nullable: false),
                    ReplyCount = table.Column<int>(nullable: false),
                    ScrollToPost = table.Column<int>(nullable: false),
                    ScrollToPostString = table.Column<string>(nullable: true),
                    StoreImageIconLocation = table.Column<string>(nullable: true),
                    StoreImageIconUrl = table.Column<string>(nullable: true),
                    TotalPages = table.Column<int>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false)
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
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AboutUser = table.Column<string>(nullable: true),
                    AimContactString = table.Column<string>(nullable: true),
                    AvatarHtml = table.Column<string>(nullable: true),
                    AvatarLink = table.Column<string>(nullable: true),
                    AvatarTitle = table.Column<string>(nullable: true),
                    CanSendPrivateMessage = table.Column<bool>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    HomePageString = table.Column<string>(nullable: true),
                    IcqContactString = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsCurrentUserPost = table.Column<bool>(nullable: false),
                    IsMod = table.Column<bool>(nullable: false),
                    LastPostDate = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    PostCount = table.Column<int>(nullable: false),
                    PostHistoryLink = table.Column<string>(nullable: true),
                    PostRate = table.Column<string>(nullable: true),
                    PrivateMessageLink = table.Column<string>(nullable: true),
                    ProfileLink = table.Column<string>(nullable: true),
                    RapSheetLink = table.Column<string>(nullable: true),
                    Roles = table.Column<string>(nullable: true),
                    SellerRating = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    YahooContactString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HasSeen = table.Column<bool>(nullable: false),
                    IsQuoting = table.Column<bool>(nullable: false),
                    PostDate = table.Column<string>(nullable: true),
                    PostDivString = table.Column<string>(nullable: true),
                    PostFormatted = table.Column<string>(nullable: true),
                    PostHtml = table.Column<string>(nullable: true),
                    PostIndex = table.Column<long>(nullable: false),
                    PostLink = table.Column<string>(nullable: true),
                    PostMarkdown = table.Column<string>(nullable: true),
                    PostQuoteLink = table.Column<string>(nullable: true),
                    PostReportLink = table.Column<string>(nullable: true),
                    ThreadId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
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
                name: "IX_Post_ThreadId",
                table: "Post",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookmarkedThreads_ForumId",
                table: "BookmarkedThreads",
                column: "ForumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "BookmarkedThreads");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
