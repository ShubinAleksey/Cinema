using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cinema.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountingReport_UserTable_EmployeeId",
                table: "AccountingReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_UserTable_BuyerId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "UserTable");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId1",
                table: "Order",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "AccountingReport",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BuyerId1",
                table: "Order",
                column: "BuyerId1");

            migrationBuilder.CreateIndex(
                name: "IX_AccountingReport_EmployeeId1",
                table: "AccountingReport",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingReport_AspNetUsers_EmployeeId1",
                table: "AccountingReport",
                column: "EmployeeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId1",
                table: "Order",
                column: "BuyerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountingReport_AspNetUsers_EmployeeId1",
                table: "AccountingReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_BuyerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BuyerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_AccountingReport_EmployeeId1",
                table: "AccountingReport");

            migrationBuilder.DropColumn(
                name: "BuyerId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "AccountingReport");

            migrationBuilder.CreateTable(
                name: "UserTable",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTable", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingReport_UserTable_EmployeeId",
                table: "AccountingReport",
                column: "EmployeeId",
                principalTable: "UserTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_UserTable_BuyerId",
                table: "Order",
                column: "BuyerId",
                principalTable: "UserTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
