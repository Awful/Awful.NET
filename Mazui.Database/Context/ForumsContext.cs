using Mazui.Core.Models.Forums;
using Mazui.Core.Models.Threads;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mazui.Database.Context
{
    public class ForumsContext : DbContext
    {
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Thread> BookmarkedThreads { get; set; }

        private string DatabasePath { get; set; }

        public ForumsContext()
        {
            DatabasePath = "Forums.sqlite";
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
        }
    }
}
