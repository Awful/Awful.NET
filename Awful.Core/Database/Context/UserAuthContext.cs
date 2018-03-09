using Awful.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awful.Database.Context
{
   public class UserAuthContext : DbContext
    {
        public DbSet<UserAuth> Users { get; set; }

        private string DatabasePath { get; set; }

        public UserAuthContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public UserAuthContext()
        {
            DatabasePath = "UserAuth.sqlite";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuth>().Ignore(b => b.AuthCookies);
        }
    }
}
