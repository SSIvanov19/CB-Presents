using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CBPresents.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isWinner",
                table: "LotteryEntries",
                newName: "IsWinner");

            migrationBuilder.CreateTable(
                name: "LotteryTimes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryTimes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotteryTimes");

            migrationBuilder.RenameColumn(
                name: "IsWinner",
                table: "LotteryEntries",
                newName: "isWinner");
        }
    }
}
