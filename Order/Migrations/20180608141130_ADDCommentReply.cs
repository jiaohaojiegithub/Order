using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Order.Migrations
{
    public partial class ADDCommentReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Forum_Name",
            //    table: "Forum",
            //    maxLength: 10,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "ArticleType_Name",
            //    table: "ArticleType",
            //    maxLength: 10,
            //    nullable: false,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Lable_ID",
            //    table: "Article",
            //    nullable: false,
            //    oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Comment_Reply",
                columns: table => new
                {
                    Comment_Reply_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment_ID = table.Column<int>(nullable: false),
                    Comment_Reply_Contene = table.Column<string>(maxLength: 500, nullable: true),
                    Comment_Reply_CreatDT = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UserCard_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment_Reply", x => x.Comment_Reply_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment_Reply");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Forum_Name",
            //    table: "Forum",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 10);

            //migrationBuilder.AlterColumn<string>(
            //    name: "ArticleType_Name",
            //    table: "ArticleType",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 10);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Lable_ID",
            //    table: "Article",
            //    nullable: false,
            //    oldClrType: typeof(string));
        }
    }
}
