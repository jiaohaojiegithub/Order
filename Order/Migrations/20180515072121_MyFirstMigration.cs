using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Order.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lable",
                columns: table => new
                {
                    Lable_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Lable_CreatDT = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    Lable_Remark = table.Column<string>(maxLength: 150, nullable: false),
                    Lable_Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lable", x => x.Lable_ID);
                });

            migrationBuilder.CreateTable(
                name: "User_Login",
                columns: table => new
                {
                    UserLogin_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserLogin_CreatDT = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UserLogin_Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    UserLogin_Name = table.Column<string>(maxLength: 20, nullable: false),
                    UserLogin_PassWord = table.Column<string>(maxLength: 18, nullable: false),
                    UserLogin_State = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Login", x => x.UserLogin_ID);
                    table.UniqueConstraint("AlternateKey_User_Guid", x => x.UserLogin_Guid);
                });

            migrationBuilder.CreateTable(
                name: "User_Card",
                columns: table => new
                {
                    UserCard_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCard_Birthday = table.Column<DateTime>(nullable: false),
                    UserCard_ChatHeadImg = table.Column<string>(nullable: true),
                    UserCard_CreatDt = table.Column<DateTime>(nullable: false),
                    UserCard_GmLevel = table.Column<int>(nullable: false),
                    UserCard_MobilPhone = table.Column<string>(nullable: true),
                    UserCard_Nickname = table.Column<string>(maxLength: 20, nullable: false),
                    UserCard_Remark = table.Column<string>(maxLength: 80, nullable: true),
                    UserCard_Sex = table.Column<string>(nullable: false),
                    UserLogin_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Card", x => x.UserCard_ID);
                    table.ForeignKey(
                        name: "FK_User_Card_User_Login",
                        column: x => x.UserLogin_ID,
                        principalTable: "User_Login",
                        principalColumn: "UserLogin_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Card_UserLogin_ID",
                table: "User_Card",
                column: "UserLogin_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lable");

            migrationBuilder.DropTable(
                name: "User_Card");

            migrationBuilder.DropTable(
                name: "User_Login");
        }
    }
}
