using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisualizzaLog.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArplog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArplogViolation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArplogId = table.Column<int>(type: "int", nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArplogViolation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArplogViolation_Arplogs_ArplogId",
                        column: x => x.ArplogId,
                        principalTable: "Arplogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArplogViolation_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArplogViolation_ArplogId",
                table: "ArplogViolation",
                column: "ArplogId");

            migrationBuilder.CreateIndex(
                name: "IX_ArplogViolation_RuleId",
                table: "ArplogViolation",
                column: "RuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArplogViolation");
        }
    }
}
