using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class Levels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slot",
                table: "PartySlot",
                newName: "ServantLevel");

            migrationBuilder.AddColumn<short>(
                name: "CraftEssenceLevel",
                table: "PartySlot",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ServantFou",
                table: "PartySlot",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servants_CollectionNo",
                table: "Servants",
                column: "CollectionNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Servants_CollectionNo",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "CraftEssenceLevel",
                table: "PartySlot");

            migrationBuilder.DropColumn(
                name: "ServantFou",
                table: "PartySlot");

            migrationBuilder.RenameColumn(
                name: "ServantLevel",
                table: "PartySlot",
                newName: "Slot");
        }
    }
}
