using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetSentry.Migrations
{
    /// <inheritdoc />
    public partial class AddLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    ActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ActionId);
                });

            migrationBuilder.CreateTable(
                name: "ObjectType",
                columns: table => new
                {
                    ObjectTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectType", x => x.ObjectTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ObjectTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "ActionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logs_ObjectType_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalTable: "ObjectType",
                        principalColumn: "ObjectTypeId");
                });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "ActionId", "Name" },
                values: new object[,]
                {
                    { "add", "Add" },
                    { "edit", "Edit" },
                    { "end", "End" },
                    { "remove", "Remove" }
                });

            migrationBuilder.InsertData(
                table: "ObjectType",
                columns: new[] { "ObjectTypeId", "Name" },
                values: new object[,]
                {
                    { "account", "Account" },
                    { "loan", "Loan" },
                    { "status", "Status" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ActionId",
                table: "Logs",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ObjectTypeId",
                table: "Logs",
                column: "ObjectTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "ObjectType");
        }
    }
}
