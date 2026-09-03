# TradeERP

A trading & distribution ERP for small and mid-sized wholesalers and distributors, built with **ASP.NET Core 8 MVC**.

## What problem does it solve?

A trading business needs to track products and stock, buy from suppliers, sell to customers, and keep its books straight — usually across a spreadsheet for inventory, a notebook for accounts, and a separate invoicing tool. Numbers drift apart, stock counts go stale, and nobody can answer "are we actually profitable this month?" without manual reconciliation.

TradeERP puts all of that in one place: every sale and purchase automatically updates stock levels and posts to the ledger, so the accounting and the inventory can never fall out of sync with each other.

## How it works

1. **Set up your data** — employees, customers, suppliers, product categories and products, and a chart of accounts.
2. **Record a bill** — a sale, a purchase, or a return. TradeERP validates it against the current accounting period, then posts it: stock moves in or out, and the matching debit/credit entry is written to the ledger automatically.
3. **Record a voucher** — a cash/bank receipt or payment, posted to the ledger the same way.
4. **Read the reports** — trial balance, customer/supplier statements of account, and stock valuation, always built from what's actually been posted, not a separate manually-maintained total.

Bilingual out of the box (English/Arabic, with right-to-left layout for Arabic), and works for a single admin or a small team with role-based access.

## Key features

- **Inventory** — products, categories, and a full stock ledger (every in/out movement, running balance, valuation)
- **Sales & purchasing** — one bill screen handles sales, purchase, and both return types, auto-numbered per your own prefix/sequence settings
- **Accounting** — journal entries, cash/bank vouchers, a real chart of accounts, opening balances, and lockable accounting periods so closed months can't be edited by accident
- **Reports** — trial balance, statement of account, and stock valuation
- **Multi-language** — English and Arabic, including RTL layout
- **Light & dark theme**, switchable and remembered per device
- **Role-based access** (Admin / Employee) with per-user account management

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 MVC |
| Database | SQL Server via Entity Framework Core 8 |
| Auth | ASP.NET Core Identity (cookie-based, role-scoped) |
| Object mapping | AutoMapper |
| Validation | FluentValidation |
| Localization | Custom JSON-backed string localizer (`Resources/en.json`, `Resources/ar.json`) |
| Frontend | Bootstrap 5, jQuery, Select2, Tabulator.js, SweetAlert2 |

## Project structure

```
TradeERP.sln
├── TradeERP.Shared      ViewModels, enums, constants — referenced by every other layer, depends on nothing
├── TradeERP.DAL         EF Core DbContext, models, repositories
├── TradeERP.BLL         Business services, validators, AutoMapper profiles
└── TradeERP.PL          MVC project: controllers, views, static assets
```

Each module (Employee, Product, BillMaster, etc.) follows the same layering: a thin controller, a service in the BLL that runs FluentValidation and orchestrates the request, and a repository in the DAL that owns the actual data/business logic for that entity.

## Getting started

**Prerequisites:** .NET 8 SDK, SQL Server (local or remote)

1. Set your connection string in `TradeERP.PL/appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TradeERP;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. Run it:
   ```bash
   dotnet restore
   dotnet run --project TradeERP.PL
   ```

   Migrations apply automatically on startup, and the database is seeded with a working demo dataset (chart of accounts, sample customers/suppliers/products, an open accounting period, a couple of posted transactions) so there's something to look at right away.

3. Log in with the seeded admin account:
   - **Email:** `admin@traderp.local`
   - **Password:** `Admin@123`

### Adding a database migration

```bash
dotnet ef migrations add <MigrationName> --project TradeERP.DAL --startup-project TradeERP.PL
```
