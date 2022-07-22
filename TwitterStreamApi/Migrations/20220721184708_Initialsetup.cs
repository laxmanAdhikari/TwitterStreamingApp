using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterStreamApi.Migrations
{
    public partial class Initialsetup : Migration
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tweet_AuthorId",
                schema: "JH.TwitterStreamingApp",
                table: "Tweet",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tweet",
                schema: "JH.TwitterStreamingApp");

            migrationBuilder.DropTable(
                name: "Author",
                schema: "JH.TwitterStreamingApp");
        }
    }
}
