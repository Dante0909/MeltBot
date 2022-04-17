using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class AliasWithSubmitter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "collectionNo",
                table: "Servants",
                newName: "CollectionNo");

            migrationBuilder.RenameColumn(
                name: "collectionNo",
                table: "CraftEssences",
                newName: "CollectionNo");

            migrationBuilder.AddColumn<int>(
                name: "SubmitterId",
                table: "ServantAliases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubmitterId",
                table: "QuestAliases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubmitterId",
                table: "MysticCodeAliases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubmitterId",
                table: "CraftEssenceAliases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServantAliases_SubmitterId",
                table: "ServantAliases",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestAliases_SubmitterId",
                table: "QuestAliases",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_MysticCodeAliases_SubmitterId",
                table: "MysticCodeAliases",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftEssenceAliases_SubmitterId",
                table: "CraftEssenceAliases",
                column: "SubmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CraftEssenceAliases_Users_SubmitterId",
                table: "CraftEssenceAliases",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MysticCodeAliases_Users_SubmitterId",
                table: "MysticCodeAliases",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestAliases_Users_SubmitterId",
                table: "QuestAliases",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServantAliases_Users_SubmitterId",
                table: "ServantAliases",
                column: "SubmitterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CraftEssenceAliases_Users_SubmitterId",
                table: "CraftEssenceAliases");

            migrationBuilder.DropForeignKey(
                name: "FK_MysticCodeAliases_Users_SubmitterId",
                table: "MysticCodeAliases");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestAliases_Users_SubmitterId",
                table: "QuestAliases");

            migrationBuilder.DropForeignKey(
                name: "FK_ServantAliases_Users_SubmitterId",
                table: "ServantAliases");

            migrationBuilder.DropIndex(
                name: "IX_ServantAliases_SubmitterId",
                table: "ServantAliases");

            migrationBuilder.DropIndex(
                name: "IX_QuestAliases_SubmitterId",
                table: "QuestAliases");

            migrationBuilder.DropIndex(
                name: "IX_MysticCodeAliases_SubmitterId",
                table: "MysticCodeAliases");

            migrationBuilder.DropIndex(
                name: "IX_CraftEssenceAliases_SubmitterId",
                table: "CraftEssenceAliases");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "ServantAliases");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "QuestAliases");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "MysticCodeAliases");

            migrationBuilder.DropColumn(
                name: "SubmitterId",
                table: "CraftEssenceAliases");

            migrationBuilder.RenameColumn(
                name: "CollectionNo",
                table: "Servants",
                newName: "collectionNo");

            migrationBuilder.RenameColumn(
                name: "CollectionNo",
                table: "CraftEssences",
                newName: "collectionNo");
        }
    }
}
