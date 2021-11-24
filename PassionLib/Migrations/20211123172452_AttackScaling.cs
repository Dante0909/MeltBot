using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class AttackScaling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short[]>(
                name: "AttackScaling",
                table: "Servants",
                type: "smallint[]",
                nullable: true);

            migrationBuilder.AddColumn<short[]>(
                name: "AttackScaling",
                table: "CraftEssences",
                type: "smallint[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackScaling",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "AttackScaling",
                table: "CraftEssences");
        }
    }
}
