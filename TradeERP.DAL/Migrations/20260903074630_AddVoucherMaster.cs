using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeERP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceVoucherMasterId",
                table: "EntryMasters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoucherMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    TreasuryLedgerAccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherMasters_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherMasters_LedgerAccounts_TreasuryLedgerAccountId",
                        column: x => x.TreasuryLedgerAccountId,
                        principalTable: "LedgerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoucherMasters_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntryMasters_SourceVoucherMasterId",
                table: "EntryMasters",
                column: "SourceVoucherMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_Code",
                table: "VoucherMasters",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_CustomerId",
                table: "VoucherMasters",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_SupplierId",
                table: "VoucherMasters",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_TreasuryLedgerAccountId",
                table: "VoucherMasters",
                column: "TreasuryLedgerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryMasters_VoucherMasters_SourceVoucherMasterId",
                table: "EntryMasters",
                column: "SourceVoucherMasterId",
                principalTable: "VoucherMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryMasters_VoucherMasters_SourceVoucherMasterId",
                table: "EntryMasters");

            migrationBuilder.DropTable(
                name: "VoucherMasters");

            migrationBuilder.DropIndex(
                name: "IX_EntryMasters_SourceVoucherMasterId",
                table: "EntryMasters");

            migrationBuilder.DropColumn(
                name: "SourceVoucherMasterId",
                table: "EntryMasters");
        }
    }
}
