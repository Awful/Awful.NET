using Awful.Parser.Models.Forums;
using Awful.Parser.Models.Threads;
using Awful.Parser.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Database.Context
{
    public class ForumsContext : DbContext
    {
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Thread> BookmarkedThreads { get; set; }

        private string DatabasePath { get; set; }

        public ForumsContext()
        {
            DatabasePath = "ForumsRedux.sqlite";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Forum>()
            .HasOne(p => p.Category)
            .WithMany(b => b.ForumList)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("ForeignKey_Forum_Category");

            modelBuilder.Entity<Forum>().HasOne(t => t.ParentForum).WithMany().HasForeignKey(t => t.ParentForumId);
            modelBuilder.Entity<Thread>().HasOne(t => t.ForumEntity).WithMany().HasForeignKey(t => t.ForumId);
        }
    }
}
