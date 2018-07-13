﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Order.Models;
using System;

namespace Order.Migrations
{
    [DbContext(typeof(DBMYBLOGContext))]
    [Migration("20180515072121_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Order.Models.DBMYBLOGClass.Lable", b =>
                {
                    b.Property<int>("Lable_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Lable_CreatDT")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Lable_Remark")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Lable_Text")
                        .IsRequired();

                    b.HasKey("Lable_ID");

                    b.ToTable("Lable");
                });

            modelBuilder.Entity("Order.Models.DBMYBLOGClass.User_Card", b =>
                {
                    b.Property<int>("UserCard_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("UserCard_Birthday");

                    b.Property<string>("UserCard_ChatHeadImg");

                    b.Property<DateTime>("UserCard_CreatDt");

                    b.Property<int>("UserCard_GmLevel");

                    b.Property<string>("UserCard_MobilPhone");

                    b.Property<string>("UserCard_Nickname")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("UserCard_Remark")
                        .HasMaxLength(80);

                    b.Property<string>("UserCard_Sex")
                        .IsRequired();

                    b.Property<int>("UserLogin_ID");

                    b.HasKey("UserCard_ID");

                    b.HasIndex("UserLogin_ID");

                    b.ToTable("User_Card");
                });

            modelBuilder.Entity("Order.Models.DBMYBLOGClass.User_Login", b =>
                {
                    b.Property<int>("UserLogin_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("UserLogin_CreatDT")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid>("UserLogin_Guid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("UserLogin_Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("UserLogin_PassWord")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<bool>("UserLogin_State");

                    b.HasKey("UserLogin_ID");

                    b.HasAlternateKey("UserLogin_Guid")
                        .HasName("AlternateKey_User_Guid");

                    b.ToTable("User_Login");
                });

            modelBuilder.Entity("Order.Models.DBMYBLOGClass.User_Card", b =>
                {
                    b.HasOne("Order.Models.DBMYBLOGClass.User_Login", "User_Login")
                        .WithMany("UserCard")
                        .HasForeignKey("UserLogin_ID")
                        .HasConstraintName("FK_User_Card_User_Login")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
