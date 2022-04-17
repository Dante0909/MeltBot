using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class Summon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastSummonCount",
                table: "Pongs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ToBePinged",
                table: "Pongs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastPongUserMention",
                table: "Cereal",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cereal_LastPongUserMention",
                table: "Cereal",
                column: "LastPongUserMention");

            migrationBuilder.AddForeignKey(
                name: "FK_Cereal_Pongs_LastPongUserMention",
                table: "Cereal",
                column: "LastPongUserMention",
                principalTable: "Pongs",
                principalColumn: "UserMention");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cereal_Pongs_LastPongUserMention",
                table: "Cereal");

            migrationBuilder.DropIndex(
                name: "IX_Cereal_LastPongUserMention",
                table: "Cereal");

            migrationBuilder.DropColumn(
                name: "LastSummonCount",
                table: "Pongs");

            migrationBuilder.DropColumn(
                name: "ToBePinged",
                table: "Pongs");

            migrationBuilder.DropColumn(
                name: "LastPongUserMention",
                table: "Cereal");
        }
    }
}
