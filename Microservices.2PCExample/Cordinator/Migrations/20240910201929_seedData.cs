using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cordinator.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("37b30f38-38af-493d-90e1-089df554c587"), "Order.API" },
                    { new Guid("67ad39fe-3dc2-45dd-a365-2df4476b63e8"), "Payment.API" },
                    { new Guid("ada5fe82-3a65-4824-acd7-474e0859992a"), "Stock.API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("37b30f38-38af-493d-90e1-089df554c587"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("67ad39fe-3dc2-45dd-a365-2df4476b63e8"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("ada5fe82-3a65-4824-acd7-474e0859992a"));
        }
    }
}
