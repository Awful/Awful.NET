using Awful.Parser.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
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

    public class UserAuth
    {
        [Key]
        public int UserAuthId { get; set; }
        public string UserName { get; set; }

        public string AvatarLink { get; set; }

        public string CookiePath { get; set; }

        public CookieContainer AuthCookies { get; set; }

        public bool IsDefaultUser { get; set; }
    }
}
