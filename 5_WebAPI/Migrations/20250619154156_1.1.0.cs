using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class _110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Substations_SubstationId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SubstationId",
                table: "Cameras",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 19, 15, 41, 55, 54, DateTimeKind.Utc).AddTicks(6990), "$2a$11$WO3fmHCsAhoYH/J.RkQlSO41hJhD1qOwwtYj91r1gspVdzLl67fvO", new DateTime(2025, 6, 19, 15, 41, 55, 54, DateTimeKind.Utc).AddTicks(6990) });

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Substations_SubstationId",
                table: "Cameras",
                column: "SubstationId",
                principalTable: "Substations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Substations_SubstationId",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubstationId",
                table: "Cameras",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Cameras",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00595b4e-6996-49ec-b197-2744ad7d53fb"),
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 17, 16, 28, 16, 836, DateTimeKind.Utc).AddTicks(3440), "$2a$11$Fi7fJjwNnpZrX8ZrmjEjGuEKyy.WiyIo.Ge5dIPOYv/ZLweyYCdIi", new DateTime(2025, 6, 17, 16, 28, 16, 836, DateTimeKind.Utc).AddTicks(3440) });

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Substations_SubstationId",
                table: "Cameras",
                column: "SubstationId",
                principalTable: "Substations",
                principalColumn: "Id");
        }
    }
}
