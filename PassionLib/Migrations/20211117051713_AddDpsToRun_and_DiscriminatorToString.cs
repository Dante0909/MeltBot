using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class AddDpsToRun_and_DiscriminatorToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiscordDiscriminator",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Dps",
                table: "Runs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dps",
                table: "Runs");

            migrationBuilder.AlterColumn<short>(
                name: "DiscordDiscriminator",
                table: "Users",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
