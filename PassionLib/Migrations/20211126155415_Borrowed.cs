using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class Borrowed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Users_SubmitterId",
                table: "Runs");

            migrationBuilder.AlterColumn<int>(
                name: "SubmitterId",
                table: "Runs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Borrowed",
                table: "PartySlot",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Runs_RunUrl",
                table: "Runs",
                column: "RunUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CraftEssences_CollectionNo",
                table: "CraftEssences",
                column: "CollectionNo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Users_SubmitterId",
                table: "Runs",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Users_SubmitterId",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_Runs_RunUrl",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_CraftEssences_CollectionNo",
                table: "CraftEssences");

            migrationBuilder.DropColumn(
                name: "Borrowed",
                table: "PartySlot");

            migrationBuilder.AlterColumn<int>(
                name: "SubmitterId",
                table: "Runs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Users_SubmitterId",
                table: "Runs",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
