using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Mazui.Database.Context;

namespace Mazui.Migrations
{
    [DbContext(typeof(UserAuthContext))]
    [Migration("20170119041039_UserDatabase")]
    partial class UserDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Mazui.Core.Models.Users.UserAuth", b =>
                {
                    b.Property<int>("UserAuthId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarLink");

                    b.Property<string>("CookiePath");

                    b.Property<string>("UserName");

                    b.HasKey("UserAuthId");

                    b.ToTable("Users");
                });
        }
    }
}
