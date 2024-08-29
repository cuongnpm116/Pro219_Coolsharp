using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_StaffStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("85ee2412-cdae-468e-ac29-e2adf4fce282"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Staffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                columns: new[] { "HashedPassword", "Status" },
                values: new object[] { "50AF425FA0E8C55EDABE13D16868C710B907FAC141C6152E68E097AC4991CDDA:56E187E7A00660AB0A5356585BEA2300:50000:SHA256", 1 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("ae3112bb-ce59-46dc-a8c6-66daa7d3d1a6"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("ae3112bb-ce59-46dc-a8c6-66daa7d3d1a6"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                column: "HashedPassword",
                value: "AB0272B5633D704E2A9AA32DB02BD75E8CC533D179E614B085652354806FB9E0:159C2CCABBF75EDBE691EA3775078FC4:50000:SHA256");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("85ee2412-cdae-468e-ac29-e2adf4fce282"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });
        }
    }
}
