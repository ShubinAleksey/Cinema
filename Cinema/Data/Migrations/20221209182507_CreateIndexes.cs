using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsRejected",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsConfirmed",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_StockReport_Name",
                table: "StockReport",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReport_Name",
                table: "PurchaseReport",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSession_MovieName",
                table: "MovieSession",
                column: "MovieName");

            migrationBuilder.CreateIndex(
                name: "IX_Department_Name",
                table: "Department",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingReport_Name",
                table: "AccountingReport",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockReport_Name",
                table: "StockReport");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReport_Name",
                table: "PurchaseReport");

            migrationBuilder.DropIndex(
                name: "IX_MovieSession_MovieName",
                table: "MovieSession");

            migrationBuilder.DropIndex(
                name: "IX_Department_Name",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_AccountingReport_Name",
                table: "AccountingReport");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRejected",
                table: "Order",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsConfirmed",
                table: "Order",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
