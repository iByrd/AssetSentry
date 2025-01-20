using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetSentry.Migrations
{
    /// <inheritdoc />
    public partial class UserAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "DeviceId", "EndDate", "IsActive", "StartDate", "Student" },
                values: new object[] { 1, 1, new DateTime(2019, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "David Miller" });
        }
    }
}
