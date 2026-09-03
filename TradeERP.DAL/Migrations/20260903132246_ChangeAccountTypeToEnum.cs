using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeERP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAccountTypeToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AccountType previously held free-text values (e.g. "Revenue", "Expense",
            // "Equity"). Normalize existing rows to the new enum's numeric values
            // before changing the column type, so live data isn't lost.
            migrationBuilder.Sql(@"
                UPDATE LedgerAccounts SET AccountType = '1' WHERE AccountType = 'Asset';
                UPDATE LedgerAccounts SET AccountType = '2' WHERE AccountType = 'Liability';
                UPDATE LedgerAccounts SET AccountType = '3' WHERE AccountType = 'Equity';
                UPDATE LedgerAccounts SET AccountType = '4' WHERE AccountType = 'Revenue';
                UPDATE LedgerAccounts SET AccountType = '5' WHERE AccountType = 'Expense';
                -- Anything unrecognized (blank/typo'd) defaults to Asset rather than
                -- failing the column type change outright.
                UPDATE LedgerAccounts SET AccountType = '1' WHERE AccountType NOT IN ('1','2','3','4','5');
            ");

            migrationBuilder.AlterColumn<int>(
                name: "AccountType",
                table: "LedgerAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "LedgerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(@"
                UPDATE LedgerAccounts SET AccountType = 'Asset' WHERE AccountType = '1';
                UPDATE LedgerAccounts SET AccountType = 'Liability' WHERE AccountType = '2';
                UPDATE LedgerAccounts SET AccountType = 'Equity' WHERE AccountType = '3';
                UPDATE LedgerAccounts SET AccountType = 'Revenue' WHERE AccountType = '4';
                UPDATE LedgerAccounts SET AccountType = 'Expense' WHERE AccountType = '5';
            ");
        }
    }
}
