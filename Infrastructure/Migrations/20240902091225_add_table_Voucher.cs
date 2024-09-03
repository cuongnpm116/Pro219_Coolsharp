using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_table_Voucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StaffRoles",
                keyColumn: "Id",
                keyValue: new Guid("b3492b52-34ba-448c-a775-91465ea1e6fd"));

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountCondition = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VoucherCode = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "StaffRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("b76ee838-b63e-4a2c-baee-10da718e3c39"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                columns: new[] { "CreatedOn", "HashedPassword" },
                values: new object[] { new DateTime(2024, 9, 2, 16, 12, 24, 394, DateTimeKind.Local).AddTicks(9059), "5FE6B5461374B2208E72B16B7F1479A0B38384FAD8D985D7F08D3E9F5DC72C8C:19782C09C7B32946C3DA6C5E9A4BD754:50000:SHA256" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "StaffRoles",
                keyColumn: "Id",
                keyValue: new Guid("b76ee838-b63e-4a2c-baee-10da718e3c39"));

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "StaffRoles",
                columns: new[] { "Id", "RoleId", "StaffId" },
                values: new object[] { new Guid("b3492b52-34ba-448c-a775-91465ea1e6fd"), new Guid("0fc1d27c-f6c4-4011-8d3c-4d33b2703369"), new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d") });

            migrationBuilder.UpdateData(
                table: "Staffs",
                keyColumn: "Id",
                keyValue: new Guid("b48703e5-2bc4-4996-88dd-4369d76fd61d"),
                columns: new[] { "CreatedOn", "HashedPassword" },
                values: new object[] { new DateTime(2024, 8, 30, 5, 46, 34, 892, DateTimeKind.Local).AddTicks(6354), "0C661030EF7376B1374F1740DE44C9B35F315E9C36481EE67B7AB06C299ADA06:CA75327244E713F685BA56EA033E5467:50000:SHA256" });
        }
    }
}
