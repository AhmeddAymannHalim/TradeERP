using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeERP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBillPostingAndLedgerLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LedgerAccountId",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceBillMasterId",
                table: "EntryMasters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerAccountId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BillType",
                table: "BillMasters",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "BillMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_LedgerAccountId",
                table: "Suppliers",
                column: "LedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryMasters_SourceBillMasterId",
                table: "EntryMasters",
                column: "SourceBillMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LedgerAccountId",
                table: "Customers",
                column: "LedgerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LedgerAccounts_LedgerAccountId",
                table: "Customers",
                column: "LedgerAccountId",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMasters_BillMasters_SourceBillMasterId",
                table: "EntryMasters",
                column: "SourceBillMasterId",
                principalTable: "BillMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_LedgerAccounts_LedgerAccountId",
                table: "Suppliers",
                column: "LedgerAccountId",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LedgerAccounts_LedgerAccountId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_EntryMasters_BillMasters_SourceBillMasterId",
                table: "EntryMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_LedgerAccounts_LedgerAccountId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_LedgerAccountId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_EntryMasters_SourceBillMasterId",
                table: "EntryMasters");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LedgerAccountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LedgerAccountId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SourceBillMasterId",
                table: "EntryMasters");

            migrationBuilder.DropColumn(
                name: "LedgerAccountId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "BillMasters");

            migrationBuilder.AlterColumn<string>(
                name: "BillType",
                table: "BillMasters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
