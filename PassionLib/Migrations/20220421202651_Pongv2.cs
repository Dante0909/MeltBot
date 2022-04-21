using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class Pongv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cereal_Pongs_LastPongUserMention",
                table: "Cereal");

            migrationBuilder.DropTable(
                name: "Pongs");

            migrationBuilder.DropIndex(
                name: "IX_Cereal_LastPongUserMention",
                table: "Cereal");

            migrationBuilder.DropColumn(
                name: "LastPongUserMention",
                table: "Cereal");

            migrationBuilder.AddColumn<decimal>(
                name: "LastPongId",
                table: "Cereal",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pongv2",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ToBePinged = table.Column<bool>(type: "boolean", nullable: true),
                    LastSummonCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pongv2", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cereal_LastPongId",
                table: "Cereal",
                column: "LastPongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cereal_Pongv2_LastPongId",
                table: "Cereal",
                column: "LastPongId",
                principalTable: "Pongv2",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cereal_Pongv2_LastPongId",
                table: "Cereal");

            migrationBuilder.DropTable(
                name: "Pongv2");

            migrationBuilder.DropIndex(
                name: "IX_Cereal_LastPongId",
                table: "Cereal");

            migrationBuilder.DropColumn(
                name: "LastPongId",
                table: "Cereal");

            migrationBuilder.AddColumn<string>(
                name: "LastPongUserMention",
                table: "Cereal",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pongs",
                columns: table => new
                {
                    UserMention = table.Column<string>(type: "text", nullable: false),
                    LastSummonCount = table.Column<int>(type: "integer", nullable: false),
                    ToBePinged = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pongs", x => x.UserMention);
                });

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
    }
}
