using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetAPI.Migrations
{
    /// <inheritdoc />
    public partial class noNeedForBalanceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceUpdateStarted",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BalanceUpdateStarted",
                table: "User",
                type: "datetime2",
                nullable: true);
        }
    }
}
