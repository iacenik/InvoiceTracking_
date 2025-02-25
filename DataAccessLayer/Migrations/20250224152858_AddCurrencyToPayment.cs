using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "PaymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Invoıces",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces",
                column: "ClientId",
                principalTable: "Clıents",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PaymentDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Invoıces",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces",
                column: "ClientId",
                principalTable: "Clıents",
                principalColumn: "ClientId");
        }
    }
}
