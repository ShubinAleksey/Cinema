using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BuyerId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BuyerId1",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "Order",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId",
                table: "Order",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "BuyerId",
                table: "Order",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId1",
                table: "Order",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BuyerId1",
                table: "Order",
                column: "BuyerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId1",
                table: "Order",
                column: "BuyerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
