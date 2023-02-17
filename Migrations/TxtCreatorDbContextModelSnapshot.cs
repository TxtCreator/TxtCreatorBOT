﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TxtCreatorBOT.Database;

#nullable disable

namespace TxtCreatorBOT.Migrations
{
    [DbContext(typeof(TxtCreatorDbContext))]
    partial class TxtCreatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("TxtCreatorBOT.Database.Models.UserModel", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Warns")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
