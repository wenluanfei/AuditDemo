using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuditDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FundName = table.Column<string>(type: "TEXT", nullable: false),
                    FinancialYear = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuditSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_AuditSessions_AuditSessionId",
                        column: x => x.AuditSessionId,
                        principalTable: "AuditSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuditSessions",
                columns: new[] { "Id", "CreatedAt", "FinancialYear", "FundName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2024-2025", "Smith Family SMSF" },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "2024-2025", "Johnson Retirement Fund" }
                });

            migrationBuilder.InsertData(
                table: "ChecklistItems",
                columns: new[] { "Id", "AuditSessionId", "Category", "Description", "IsCompleted", "Notes" },
                values: new object[,]
                {
                    { 1, 1, "Member Information", "Verify member details and TFNs", false, "" },
                    { 2, 1, "Member Information", "Confirm member contributions within caps", false, "" },
                    { 3, 1, "Member Information", "Check pension payment minimums met", false, "" },
                    { 4, 1, "Financial Statements", "Review financial statements for accuracy", false, "" },
                    { 5, 1, "Financial Statements", "Verify opening balances match prior year", false, "" },
                    { 6, 1, "Financial Statements", "Confirm investment income correctly recorded", false, "" },
                    { 7, 1, "Investments", "Confirm investments held in fund's name", false, "" },
                    { 8, 1, "Investments", "Check sole purpose test compliance", false, "" },
                    { 9, 1, "Investments", "Verify no in-house assets exceed 5% limit", false, "" },
                    { 10, 1, "Compliance", "Confirm trust deed is up to date", false, "" },
                    { 11, 1, "Compliance", "Check audit report lodged within deadlines", false, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_AuditSessionId",
                table: "ChecklistItems",
                column: "AuditSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "AuditSessions");
        }
    }
}
