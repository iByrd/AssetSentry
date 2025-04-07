using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetSentry.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLoanDeviceRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loans_DeviceId",
                table: "Loans");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_DeviceId",
                table: "Loans",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loans_DeviceId",
                table: "Loans");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_DeviceId",
                table: "Loans",
                column: "DeviceId",
                unique: true);
        }
    }
}
