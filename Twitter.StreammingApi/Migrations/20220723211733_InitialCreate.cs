using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twitter.StreammingApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "JH.TwitterStreamingApp");

            migrationBuilder.CreateTable(
                name: "Author",
                schema: "JH.TwitterStreamingApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TwwitterAuthorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TwitterName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterHandle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HashTag",
                schema: "JH.TwitterStreamingApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TweeterTweetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashTagName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tweet",
                schema: "JH.TwitterStreamingApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    TweeterTweetId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TwitterPublished = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    HashTagId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tweet_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "JH.TwitterStreamingApp",
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tweet_HashTag_HashTagId",
                        column: x => x.HashTagId,
                        principalSchema: "JH.TwitterStreamingApp",
                        principalTable: "HashTag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tweet_AuthorId",
                schema: "JH.TwitterStreamingApp",
                table: "Tweet",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tweet_HashTagId",
                schema: "JH.TwitterStreamingApp",
                table: "Tweet",
                column: "HashTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tweet",
                schema: "JH.TwitterStreamingApp");

            migrationBuilder.DropTable(
                name: "Author",
                schema: "JH.TwitterStreamingApp");

            migrationBuilder.DropTable(
                name: "HashTag",
                schema: "JH.TwitterStreamingApp");
        }
    }
}
