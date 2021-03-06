﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Kixify.OnFeet.Dal;
using Kixify.OnFeet.Dal.Entity;

namespace Kixify.OnFeet.Dal.Migrations
{
    [DbContext(typeof(OnFeetContext))]
    partial class OnFeetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kixify.OnFeet.Dal.Entity.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("DeviceToken");

                    b.Property<int>("DeviceType");

                    b.Property<long?>("EventId");

                    b.Property<string>("ImageUrl");

                    b.Property<int>("Platform");

                    b.Property<string>("ProfileUrl");

                    b.Property<string>("Sku");

                    b.Property<string>("Title");

                    b.Property<long>("UserId");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Kixify.OnFeet.Dal.Entity.Like", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ImageId");

                    b.Property<int>("Platform");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Kixify.OnFeet.Dal.Entity.Like", b =>
                {
                    b.HasOne("Kixify.OnFeet.Dal.Entity.Image", "Image")
                        .WithMany("Likes")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
