using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class DpsCancer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Servants_DpsId",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_Runs_DpsId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "DpsId",
                table: "Runs");

            migrationBuilder.AddColumn<bool>(
                name: "Dps_Borrowed",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dps_CraftEssenceId",
                table: "Runs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Dps_CraftEssenceLevel",
                table: "Runs",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Dps_CraftEssenceMlb",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Dps_ServantFou",
                table: "Runs",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dps_ServantId",
                table: "Runs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Dps_ServantLevel",
                table: "Runs",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Dps_TotalAttack",
                table: "Runs",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Runs_Dps_CraftEssenceId",
                table: "Runs",
                column: "Dps_CraftEssenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_Dps_ServantId",
                table: "Runs",
                column: "Dps_ServantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_CraftEssences_Dps_CraftEssenceId",
                table: "Runs",
                column: "Dps_CraftEssenceId",
                principalTable: "CraftEssences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Servants_Dps_ServantId",
                table: "Runs",
                column: "Dps_ServantId",
                principalTable: "Servants",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Runs_CraftEssences_Dps_CraftEssenceId",
                table: "Runs");

            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Servants_Dps_ServantId",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_Runs_Dps_CraftEssenceId",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_Runs_Dps_ServantId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_Borrowed",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_CraftEssenceId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_CraftEssenceLevel",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_CraftEssenceMlb",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_ServantFou",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_ServantId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_ServantLevel",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Dps_TotalAttack",
                table: "Runs");

            migrationBuilder.AddColumn<int>(
                name: "DpsId",
                table: "Runs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Runs_DpsId",
                table: "Runs",
                column: "DpsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Servants_DpsId",
                table: "Runs",
                column: "DpsId",
                principalTable: "Servants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
