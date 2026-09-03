using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeERP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryTypeToEntryMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntryType",
                table: "EntryMasters",
                type: "int",
                nullable: false,
                defaultValue: 1); // EntryType.Manual

            migrationBuilder.Sql(
                "UPDATE [EntryMasters] SET [EntryType] = 2 WHERE [SourceBillMasterId] IS NOT NULL;"); // BillPosting
            migrationBuilder.Sql(
                "UPDATE [EntryMasters] SET [EntryType] = 3 WHERE [SourceVoucherMasterId] IS NOT NULL;"); // VoucherPosting
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryType",
                table: "EntryMasters");
        }
    }
}
