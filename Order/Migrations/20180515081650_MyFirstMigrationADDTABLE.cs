using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Order.Migrations
{
    public partial class MyFirstMigrationADDTABLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Article_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleType_ID = table.Column<int>(nullable: false),
                    Article_Abstract = table.Column<string>(nullable: true),
                    Article_Content = table.Column<string>(nullable: true),
                    Article_CreateDT = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    Article_Img = table.Column<string>(nullable: true),
                    Article_Title = table.Column<string>(nullable: true),
                    Lable_ID = table.Column<int>(nullable: false),
                    UserCard_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Article_ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticleType",
                columns: table => new
                {
                    ArticleType_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleType_Name = table.Column<string>(nullable: false),
                    Forum_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleType", x => x.ArticleType_ID);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Comment_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Article_ID = table.Column<int>(nullable: false),
                    Comment_Contene = table.Column<string>(maxLength: 500, nullable: true),
                    Comment_CreatDT = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UserCard_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Comment_ID);
                });

            migrationBuilder.CreateTable(
                name: "Forum",
                columns: table => new
                {
                    Forum_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Forum_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.Forum_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "ArticleType");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Forum");
        }
    }
}
