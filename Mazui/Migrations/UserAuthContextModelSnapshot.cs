using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Mazui.Database.Context;

namespace Mazui.Migrations
{
    [DbContext(typeof(UserAuthContext))]
    partial class UserAuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Mazui.Core.Models.Users.UserAuth", b =>
                {
                    b.Property<int>("UserAuthId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarLink");

                    b.Property<string>("CookiePath");

                    b.Property<bool>("IsDefaultUser");

                    b.Property<string>("UserName");

                    b.HasKey("UserAuthId");

                    b.ToTable("Users");
                });
        }
    }
}
