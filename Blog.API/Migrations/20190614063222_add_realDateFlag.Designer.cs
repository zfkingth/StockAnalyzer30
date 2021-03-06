﻿// <auto-generated />
using System;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.API.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20190614063222_add_realDateFlag")]
    partial class add_realDateFlag
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Blog.Model.DayData", b =>
                {
                    b.Property<string>("StockId")
                        .HasMaxLength(10);

                    b.Property<DateTime>("Date");

                    b.Property<float>("Amount");

                    b.Property<float>("Close");

                    b.Property<float>("High");

                    b.Property<float?>("HuanShouLiu");

                    b.Property<float?>("LiuTongShiZhi");

                    b.Property<float>("Low");

                    b.Property<float>("Open");

                    b.Property<int>("Type");

                    b.Property<float>("Volume");

                    b.Property<float?>("ZhangDieFu");

                    b.Property<float?>("ZongShiZhi");

                    b.HasKey("StockId", "Date");

                    b.ToTable("DayDataSet");
                });

            modelBuilder.Entity("Blog.Model.Like", b =>
                {
                    b.Property<string>("StoryId");

                    b.Property<string>("UserId");

                    b.HasKey("StoryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("Blog.Model.Message", b =>
                {
                    b.Property<DateTime>("MesTime");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(2048);

                    b.HasKey("MesTime");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Blog.Model.RealTimeData", b =>
                {
                    b.Property<string>("StockId");

                    b.Property<DateTime>("Date")
                        .HasMaxLength(20);

                    b.Property<float>("Amount");

                    b.Property<float>("Close");

                    b.Property<float>("High");

                    b.Property<float?>("HuanShouLiu");

                    b.Property<float?>("LiuTongShiZhi");

                    b.Property<float>("Low");

                    b.Property<float>("Open");

                    b.Property<string>("StockName")
                        .HasMaxLength(10);

                    b.Property<float>("Volume");

                    b.Property<float?>("ZhangDieFu");

                    b.Property<float?>("ZongShiZhi");

                    b.HasKey("StockId", "Date");

                    b.ToTable("RealTimeDataSet");
                });

            modelBuilder.Entity("Blog.Model.SearchResult", b =>
                {
                    b.Property<string>("ActionName")
                        .HasMaxLength(32);

                    b.Property<string>("ActionParams")
                        .HasMaxLength(512);

                    b.Property<DateTime>("ActionDate");

                    b.Property<string>("ActionReslut")
                        .HasMaxLength(4096);

                    b.HasKey("ActionName", "ActionParams", "ActionDate");

                    b.ToTable("SearchResultSet");
                });

            modelBuilder.Entity("Blog.Model.Share", b =>
                {
                    b.Property<string>("StoryId");

                    b.Property<string>("UserId");

                    b.HasKey("StoryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Share");
                });

            modelBuilder.Entity("Blog.Model.Sharing", b =>
                {
                    b.Property<string>("StockId")
                        .HasMaxLength(10);

                    b.Property<DateTime>("DateGongGao");

                    b.Property<DateTime?>("DateChuXi");

                    b.Property<DateTime?>("DateDengJi");

                    b.Property<float>("PaiXi");

                    b.Property<float>("SongGu");

                    b.Property<float>("ZhuanZeng");

                    b.HasKey("StockId", "DateGongGao");

                    b.ToTable("SharingSet");
                });

            modelBuilder.Entity("Blog.Model.Stock", b =>
                {
                    b.Property<string>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10);

                    b.Property<DateTime>("RealDataUpdated");

                    b.Property<string>("StockName")
                        .HasMaxLength(10);

                    b.Property<int>("StockType");

                    b.HasKey("StockId");

                    b.ToTable("StockSet");
                });

            modelBuilder.Entity("Blog.Model.StockEvent", b =>
                {
                    b.Property<string>("EventName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30);

                    b.Property<DateTime?>("LastAriseEndDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(null);

                    b.Property<DateTime>("LastAriseStartDate");

                    b.Property<int>("Status");

                    b.HasKey("EventName");

                    b.ToTable("StockEvents");
                });

            modelBuilder.Entity("Blog.Model.StockNum", b =>
                {
                    b.Property<string>("StockId")
                        .HasMaxLength(10);

                    b.Property<DateTime>("Date");

                    b.Property<double>("All");

                    b.Property<double>("LiuTongA");

                    b.HasKey("StockId", "Date");

                    b.ToTable("StockNumSet");
                });

            modelBuilder.Entity("Blog.Model.Story", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<long>("CreationTime");

                    b.Property<bool>("Draft");

                    b.Property<long>("LastEditTime");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<long>("PublishTime");

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Story");
                });

            modelBuilder.Entity("Blog.Model.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(40);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<DateTime>("ExpiredDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(90);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasDefaultValue("user");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Blog.Model.DayData", b =>
                {
                    b.HasOne("Blog.Model.Stock", "Stock")
                        .WithMany("DayDataSet")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.Like", b =>
                {
                    b.HasOne("Blog.Model.Story", "Story")
                        .WithMany("Likes")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Blog.Model.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.RealTimeData", b =>
                {
                    b.HasOne("Blog.Model.Stock", "Stock")
                        .WithMany("RealTimeDataSet")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.Share", b =>
                {
                    b.HasOne("Blog.Model.Story", "Story")
                        .WithMany("Shares")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Blog.Model.User", "User")
                        .WithMany("Shares")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.Sharing", b =>
                {
                    b.HasOne("Blog.Model.Stock", "Stock")
                        .WithMany("SharingSet")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.StockNum", b =>
                {
                    b.HasOne("Blog.Model.Stock", "Stock")
                        .WithMany("StockNumSet")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Blog.Model.Story", b =>
                {
                    b.HasOne("Blog.Model.User", "Owner")
                        .WithMany("Stories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
