using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class _120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentDeliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Permalink = table.Column<string>(type: "text", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDeliveries", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 23, 39, 24, 192, DateTimeKind.Utc).AddTicks(4870), "$2a$11$aGO5PeK112k4HqYq8ChBHuCRpOWTd8ngQauR8nZDK0cxnkm9geE9O", new DateTime(2025, 6, 19, 23, 39, 24, 192, DateTimeKind.Utc).AddTicks(4870) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentDeliveries");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 15, 41, 55, 54, DateTimeKind.Utc).AddTicks(6990), "$2a$11$WO3fmHCsAhoYH/J.RkQlSO41hJhD1qOwwtYj91r1gspVdzLl67fvO", new DateTime(2025, 6, 19, 15, 41, 55, 54, DateTimeKind.Utc).AddTicks(6990) });
        }
    }
}
