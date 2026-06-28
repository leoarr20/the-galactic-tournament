using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGalacticTournament.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PowerLevel = table.Column<int>(type: "int", nullable: false),
                    SpecialAbility = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeciesAId = table.Column<int>(type: "int", nullable: false),
                    SpeciesBId = table.Column<int>(type: "int", nullable: false),
                    WinnerSpeciesId = table.Column<int>(type: "int", nullable: false),
                    BattleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Species_SpeciesAId",
                        column: x => x.SpeciesAId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Battles_Species_SpeciesBId",
                        column: x => x.SpeciesBId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Battles_Species_WinnerSpeciesId",
                        column: x => x.WinnerSpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battles_SpeciesAId",
                table: "Battles",
                column: "SpeciesAId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_SpeciesBId",
                table: "Battles",
                column: "SpeciesBId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_WinnerSpeciesId",
                table: "Battles",
                column: "WinnerSpeciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
