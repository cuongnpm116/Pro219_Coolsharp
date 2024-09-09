using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_table_payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "StaffRoles",
                keyColumn: "Id",
                keyValue: new Guid("b76ee838-b63e-4a2c-baee-10da718e3c39"));

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Payments");

            migrationBuilder.InsertData(
                table: "StaffRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("5a52bc0c-30c9-4a3a-974d-ca488c995720"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                columns: new[] { "CreatedOn", "HashedPassword", "ImageUrl" },
                values: new object[] { new DateTime(2024, 9, 8, 15, 38, 37, 422, DateTimeKind.Local).AddTicks(2880), "1726B4D057981DAC4302F563C5CB78F2B5F4798B7F18D4322FCA27681FA44F5D:6A54BCE5CAF2F3024BC016AF2AEC6D60:50000:SHA256", "default-profile.png" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "StaffRoles",
                keyColumn: "Id",
                keyValue: new Guid("5a52bc0c-30c9-4a3a-974d-ca488c995720"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "StaffRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("b76ee838-b63e-4a2c-baee-10da718e3c39"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                columns: new[] { "CreatedOn", "HashedPassword", "ImageUrl" },
                values: new object[] { new DateTime(2024, 9, 2, 16, 12, 24, 394, DateTimeKind.Local).AddTicks(9059), "5FE6B5461374B2208E72B16B7F1479A0B38384FAD8D985D7F08D3E9F5DC72C8C:19782C09C7B32946C3DA6C5E9A4BD754:50000:SHA256", "default.png" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
