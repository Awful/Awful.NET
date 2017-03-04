using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Mazui.Database.Context;

namespace Mazui.Migrations.Forums
{
    [DbContext(typeof(ForumsContext))]
    [Migration("20170119180655_Forums")]
    partial class Forums
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Mazui.Core.Models.Forums.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Mazui.Core.Models.Forums.Forum", b =>
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

                    b.Property<int>("TotalPages");

                    b.HasKey("ForumId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("Mazui.Core.Models.Forums.Forum", b =>
                {
                    b.HasOne("Mazui.Core.Models.Forums.Category", "Category")
                        .WithMany("ForumList")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ForeignKey_Forum_Category")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
