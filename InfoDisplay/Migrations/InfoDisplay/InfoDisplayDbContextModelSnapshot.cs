﻿// <auto-generated />
using System;
using InfoDisplay.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InfoDisplay.Migrations.InfoDisplay
{
    [DbContext(typeof(InfoDisplayDbContext))]
    partial class InfoDisplayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoard", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Uid");

                    b.ToTable("InfoBoards");
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItem", b =>
                {
                    b.Property<Guid>("Uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InfoBoardUid")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Uid");

                    b.HasIndex("InfoBoardUid");

                    b.ToTable("InfoBoardItems");

                    b.HasDiscriminator<int>("Type").HasValue(4);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItemImage", b =>
                {
                    b.HasBaseType("Gitec.Models.InfoDisplay.InfoBoardItem");

                    b.Property<string>("ImageAlt")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageCaption")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageStory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItemMarkdown", b =>
                {
                    b.HasBaseType("Gitec.Models.InfoDisplay.InfoBoardItem");

                    b.Property<string>("MarkdownContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItemRssFeed", b =>
                {
                    b.HasBaseType("Gitec.Models.InfoDisplay.InfoBoardItem");

                    b.Property<string>("RssFeedUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue(5);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItemText", b =>
                {
                    b.HasBaseType("Gitec.Models.InfoDisplay.InfoBoardItem");

                    b.Property<string>("TextContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItemVideo", b =>
                {
                    b.HasBaseType("Gitec.Models.InfoDisplay.InfoBoardItem");

                    b.Property<string>("VideoAlt")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("VideoCaption")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("VideoSource")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("VideoStory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoardItem", b =>
                {
                    b.HasOne("Gitec.Models.InfoDisplay.InfoBoard", null)
                        .WithMany("InfoBoardItems")
                        .HasForeignKey("InfoBoardUid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Gitec.Models.InfoDisplay.InfoBoard", b =>
                {
                    b.Navigation("InfoBoardItems");
                });
#pragma warning restore 612, 618
        }
    }
}
