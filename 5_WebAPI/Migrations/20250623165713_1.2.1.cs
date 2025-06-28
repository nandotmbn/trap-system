using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class _121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketNumber",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 23, 16, 57, 12, 228, DateTimeKind.Utc).AddTicks(7440), "$2a$11$TFV40uZL7DVAnDXJKWnKnuzEgIfVHrewFz2339.HaL4QJW6GbB8Kq", new DateTime(2025, 6, 23, 16, 57, 12, 228, DateTimeKind.Utc).AddTicks(7440) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "Tickets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 23, 39, 24, 192, DateTimeKind.Utc).AddTicks(4870), "$2a$11$aGO5PeK112k4HqYq8ChBHuCRpOWTd8ngQauR8nZDK0cxnkm9geE9O", new DateTime(2025, 6, 19, 23, 39, 24, 192, DateTimeKind.Utc).AddTicks(4870) });
        }
    }
}
