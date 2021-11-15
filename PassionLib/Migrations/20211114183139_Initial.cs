using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CraftEssences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    JpName = table.Column<string>(type: "text", nullable: false),
                    NaName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftEssences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MysticCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    JpName = table.Column<string>(type: "text", nullable: false),
                    NaName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MysticCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    JpName = table.Column<string>(type: "text", nullable: false),
                    NaName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    JpName = table.Column<string>(type: "text", nullable: false),
                    NaName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordUsername = table.Column<string>(type: "text", nullable: true),
                    DiscordDiscriminator = table.Column<short>(type: "smallint", nullable: true),
                    DiscordSnowflake = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CraftEssenceAliases",
                columns: table => new
                {
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    CraftEssenceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftEssenceAliases", x => x.Nickname);
                    table.ForeignKey(
                        name: "FK_CraftEssenceAliases_CraftEssences_CraftEssenceId",
                        column: x => x.CraftEssenceId,
                        principalTable: "CraftEssences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MysticCodeAliases",
                columns: table => new
                {
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    MysticCodeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MysticCodeAliases", x => x.Nickname);
                    table.ForeignKey(
                        name: "FK_MysticCodeAliases_MysticCodes_MysticCodeId",
                        column: x => x.MysticCodeId,
                        principalTable: "MysticCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestAliases",
                columns: table => new
                {
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    QuestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestAliases", x => x.Nickname);
                    table.ForeignKey(
                        name: "FK_QuestAliases_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServantAliases",
                columns: table => new
                {
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    ServantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServantAliases", x => x.Nickname);
                    table.ForeignKey(
                        name: "FK_ServantAliases_Servants_ServantId",
                        column: x => x.ServantId,
                        principalTable: "Servants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestId = table.Column<int>(type: "integer", nullable: false),
                    Phase = table.Column<short>(type: "smallint", nullable: true),
                    MysticCodeId = table.Column<int>(type: "integer", nullable: true),
                    RunDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RunUrl = table.Column<string>(type: "text", nullable: false),
                    CsUsed = table.Column<int>(type: "integer", nullable: true),
                    RevivesUsed = table.Column<int>(type: "integer", nullable: true),
                    Misc = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmitterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Runs_MysticCodes_MysticCodeId",
                        column: x => x.MysticCodeId,
                        principalTable: "MysticCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Runs_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Runs_Users_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PartySlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServantId = table.Column<int>(type: "integer", nullable: true),
                    CraftEssenceId = table.Column<int>(type: "integer", nullable: true),
                    CraftEssenceMlb = table.Column<bool>(type: "boolean", nullable: true),
                    RunId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartySlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartySlot_CraftEssences_CraftEssenceId",
                        column: x => x.CraftEssenceId,
                        principalTable: "CraftEssences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartySlot_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartySlot_Servants_ServantId",
                        column: x => x.ServantId,
                        principalTable: "Servants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CraftEssenceAliases_CraftEssenceId",
                table: "CraftEssenceAliases",
                column: "CraftEssenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MysticCodeAliases_MysticCodeId",
                table: "MysticCodeAliases",
                column: "MysticCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartySlot_CraftEssenceId",
                table: "PartySlot",
                column: "CraftEssenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartySlot_RunId",
                table: "PartySlot",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_PartySlot_ServantId",
                table: "PartySlot",
                column: "ServantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestAliases_QuestId",
                table: "QuestAliases",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_MysticCodeId",
                table: "Runs",
                column: "MysticCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_QuestId",
                table: "Runs",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Runs_SubmitterId",
                table: "Runs",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ServantAliases_ServantId",
                table: "ServantAliases",
                column: "ServantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CraftEssenceAliases");

            migrationBuilder.DropTable(
                name: "MysticCodeAliases");

            migrationBuilder.DropTable(
                name: "PartySlot");

            migrationBuilder.DropTable(
                name: "QuestAliases");

            migrationBuilder.DropTable(
                name: "ServantAliases");

            migrationBuilder.DropTable(
                name: "CraftEssences");

            migrationBuilder.DropTable(
                name: "Runs");

            migrationBuilder.DropTable(
                name: "Servants");

            migrationBuilder.DropTable(
                name: "MysticCodes");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
