using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bet_EventId",
                table: "Bet",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_UserId",
                table: "Bet",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_Event_EventId",
                table: "Bet",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bet_User_UserId",
                table: "Bet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bet_Event_EventId",
                table: "Bet");

            migrationBuilder.DropForeignKey(
                name: "FK_Bet_User_UserId",
                table: "Bet");

            migrationBuilder.DropIndex(
                name: "IX_Bet_EventId",
                table: "Bet");

            migrationBuilder.DropIndex(
                name: "IX_Bet_UserId",
                table: "Bet");
        }
    }
}
