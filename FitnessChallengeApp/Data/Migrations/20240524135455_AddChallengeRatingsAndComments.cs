using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessChallengeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChallengeRatingsAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeComments_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeRatings_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteChallenges_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeComments_ChallengeId",
                table: "ChallengeComments",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeComments_UserId",
                table: "ChallengeComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRatings_ChallengeId",
                table: "ChallengeRatings",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRatings_UserId",
                table: "ChallengeRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteChallenges_ChallengeId",
                table: "FavoriteChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteChallenges_UserId",
                table: "FavoriteChallenges",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeComments");

            migrationBuilder.DropTable(
                name: "ChallengeRatings");

            migrationBuilder.DropTable(
                name: "FavoriteChallenges");
        }
    }
}
