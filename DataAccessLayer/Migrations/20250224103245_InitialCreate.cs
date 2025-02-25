using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "Clıents");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Clıents",
                newName: "CompanyName");

            migrationBuilder.AddColumn<string>(
                name: "SoldProducts",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Invoıces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Clıents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Clıents",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Clıents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientCode",
                table: "Clıents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Clıents",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_ItemId",
                table: "InvoiceDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EmployeeId",
                table: "Expenses",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetails_ItemId",
                table: "InvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_EmployeeId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SoldProducts",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Invoıces");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Clıents");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Clıents");

            migrationBuilder.DropColumn(
                name: "ClientCode",
                table: "Clıents");

            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Clıents");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Clıents",
                newName: "ClientName");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "PaymentDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PaymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PaymentDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "InvoiceDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Clıents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "Clıents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
