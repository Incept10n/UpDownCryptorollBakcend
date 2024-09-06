using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WalletAddress = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CurrentBalance = table.Column<float>(type: "real", nullable: false),
                    LoginStreakCount = table.Column<int>(type: "integer", nullable: false),
                    CurrentMatchId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Coin = table.Column<int>(type: "integer", nullable: false),
                    EntryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EntryPrice = table.Column<float>(type: "real", nullable: false),
                    Prediction = table.Column<int>(type: "integer", nullable: false),
                    PredictionTimeframe = table.Column<TimeSpan>(type: "interval", nullable: false),
                    PredictionAmount = table.Column<float>(type: "real", nullable: false),
                    ExitTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExitPrice = table.Column<float>(type: "real", nullable: true),
                    Res = table.Column<int>(type: "integer", nullable: true),
                    ResultPayout = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
