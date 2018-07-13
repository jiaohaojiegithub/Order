using Microsoft.EntityFrameworkCore;
using Order.Models.DBMYBLOGClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Order.Models
{
    public class DBMYBLOGContext : DbContext
    {
        //public DBMYBLOGContext(DbContextOptions<DBMYBLOGContext> options) : base(options)
        //{

        //}
        //public DBMYBLOGContext(DbContextOptions<DBMYBLOGContext> options) : base(options)
        //{
        //    //基类构造函数参数就是web.config配置的节点名称
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBMYBLOGDatabase"].ConnectionString);//@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;"
            
        }
        public DbSet<User_Login> User_Login { get; set; }
        public DbSet<User_Card> User_Card { get; set; }
        public DbSet<Lable> Lable { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Forum> Forum { get; set; }
        public DbSet<ArticleType> ArticleType { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Comment_Reply> Comment_Reply { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Login>(entity =>
            {
                entity.Property(e => e.UserLogin_CreatDT).HasDefaultValueSql("GETDATE()");
                entity.HasKey(e => e.UserLogin_ID);
                //entity.Property(e => e.UserLogin_Guid).ValueGeneratedOnAdd();
                entity.Property(e => e.UserLogin_Guid).HasDefaultValueSql("NEWID()");
                entity.HasAlternateKey(e => e.UserLogin_Guid).HasName("AlternateKey_User_Guid"); //唯一约束
            });
            modelBuilder.Entity<User_Card>(entity =>
            {
                entity.HasKey(e => e.UserCard_ID);
                entity.HasOne(d => d.User_Login)
                .WithMany(p => p.UserCard)
                .HasForeignKey(d => d.UserLogin_ID)
                .HasConstraintName("FK_User_Card_User_Login");
                //entity.Property(e => e.UserCard_CreatDt).HasDefaultValueSql("GETDATE()");//新数据库迁移时变更
            });
            modelBuilder.Entity<Lable>(entity =>
            {
                entity.HasKey(c => c.Lable_ID);
                entity.Property(e => e.Lable_CreatDT).HasDefaultValueSql("GETDATE()");

            });
            modelBuilder.Entity<ArticleType>(entity =>
            {
                entity.HasKey(e => e.ArticleType_ID);
            });
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Article_ID);
                entity.Property(e => e.Article_CreateDT).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Forum>(entity =>
            {
                entity.HasKey(e => e.Forum_ID);
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Comment_ID);
                entity.Property(e => e.Comment_CreatDT).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Comment_Reply>(entity =>
            {
                entity.HasKey(e => e.Comment_Reply_ID);
                entity.Property(e => e.Comment_Reply_CreatDT).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}