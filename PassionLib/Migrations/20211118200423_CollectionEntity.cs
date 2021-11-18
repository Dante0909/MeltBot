using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class CollectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dps",
                table: "Runs");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Servants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Cost",
                table: "Runs",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DpsId",
                table: "Runs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Failure",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoCeDps",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoDupe",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Rta",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ServantCount",
                table: "Runs",
                type: "smallint",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Servants_DpsId",
                table: "Runs");

            migrationBuilder.DropIndex(
                name: "IX_Runs_DpsId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "DpsId",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Failure",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "NoCeDps",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "NoDupe",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Rta",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "ServantCount",
                table: "Runs");

            migrationBuilder.AddColumn<short>(
                name: "Dps",
                table: "Runs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
