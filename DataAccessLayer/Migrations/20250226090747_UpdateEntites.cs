using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoıces_Employees_EmployeeId",
                table: "Invoıces");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoıces_ExpenseCategories_CategoryId",
                table: "Invoıces");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Invoıces_InvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clıents_ClientId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoıces",
                table: "Invoıces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clıents",
                table: "Clıents");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "Invoıces",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "Clıents",
                newName: "Clients");

            migrationBuilder.RenameIndex(
                name: "IX_Invoıces_EmployeeId",
                table: "Invoices",
                newName: "IX_Invoices_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoıces_ClientId",
                table: "Invoices",
                newName: "IX_Invoices_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoıces_CategoryId",
                table: "Invoices",
                newName: "IX_Invoices_CategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "InvoiceDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SoldProducts",
                table: "InvoiceDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Employees_EmployeeId",
                table: "Invoices",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_ExpenseCategories_CategoryId",
                table: "Invoices",
                column: "CategoryId",
                principalTable: "ExpenseCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Employees_EmployeeId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_ExpenseCategories_CategoryId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SoldProducts",
                table: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoıces");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Clıents");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_EmployeeId",
                table: "Invoıces",
                newName: "IX_Invoıces_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoıces",
                newName: "IX_Invoıces_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CategoryId",
                table: "Invoıces",
                newName: "IX_Invoıces_CategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "InvoiceDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoıces",
                table: "Invoıces",
                column: "InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clıents",
                table: "Clıents",
                column: "ClientId");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Invoıces_Clıents_ClientId",
                table: "Invoıces",
                column: "ClientId",
                principalTable: "Clıents",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoıces_Employees_EmployeeId",
                table: "Invoıces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoıces_ExpenseCategories_CategoryId",
                table: "Invoıces",
                column: "CategoryId",
                principalTable: "ExpenseCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Invoıces_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId",
                principalTable: "Invoıces",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Items_ItemId",
                table: "InvoiceDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clıents_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Clıents",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
