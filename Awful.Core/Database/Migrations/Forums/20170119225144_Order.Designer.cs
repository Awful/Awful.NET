using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Awful.Database.Context;

namespace Awful.Migrations.Forums
{
    [DbContext(typeof(ForumsContext))]
    [Migration("20170119225144_Order")]
    partial class Order
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Awful.Models.Forums.Forum", b =>
                {
                    b.HasOne("Awful.Models.Forums.Category", "Category")
                        .WithMany("ForumList")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("ForeignKey_Forum_Category")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
