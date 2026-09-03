using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeERP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCodeConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoucherMasters_Code",
                table: "VoucherMasters");

            migrationBuilder.DropIndex(
                name: "IX_EntryMasters_Code",
                table: "EntryMasters");

            migrationBuilder.DropIndex(
                name: "IX_BillMasters_Code",
                table: "BillMasters");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_Code",
                table: "VoucherMasters",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntryMasters_Code",
                table: "EntryMasters",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillMasters_Code",
                table: "BillMasters",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoucherMasters_Code",
                table: "VoucherMasters");

            migrationBuilder.DropIndex(
                name: "IX_EntryMasters_Code",
                table: "EntryMasters");

            migrationBuilder.DropIndex(
                name: "IX_BillMasters_Code",
                table: "BillMasters");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherMasters_Code",
                table: "VoucherMasters",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_EntryMasters_Code",
                table: "EntryMasters",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_BillMasters_Code",
                table: "BillMasters",
                column: "Code");
        }
    }
}
