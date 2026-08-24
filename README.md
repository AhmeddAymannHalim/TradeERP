# TradeERP

A Clean Architecture skeleton for a Trading & Distribution ERP system, built on **ASP.NET Core 8 MVC**.

> This repository currently contains **infrastructure only** — no business entities (Employee, Product, etc.) have been implemented yet. It is the foundation the real modules will be built on top of.

---

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core 8.0 MVC |
| ORM | Entity Framework Core 8 (SQL Server provider) |
| Auth | ASP.NET Core Identity (cookie-based, role scaffolding wired) |
| Object mapping | AutoMapper 12 |
| Localization | Custom JSON-file string localizer (`Resources/en.json`, `Resources/ar.json`) — no `.resx` |
| Frontend | Bootstrap 5, jQuery, SweetAlert2, Select2, Tabulator.js (all via CDN) |
| Caching | `IMemoryCache` |

---

## Solution Structure (Clean Architecture)

```
TradeERP.sln
├── TradeERP.Shared      Cross-cutting: ViewModels, Enums, Options, Constants, Extensions, HelperServices
├── TradeERP.DAL         EF Core DbContext, combined Repositories, Unit of Work
├── TradeERP.BLL         Application services, AutoMapper profiles, DI registration
└── TradeERP.PL          MVC project: Controllers, Views, wwwroot, composition root
```

### Dependency direction

```
TradeERP.PL  →  TradeERP.BLL  →  TradeERP.DAL  →  TradeERP.Shared
     └──────────────────────────────────────────────────┘
```

`TradeERP.Shared` has no project references of its own — every other layer can reference it, but it never references back, so there is no way to introduce a circular dependency.

| Project | References | Notable packages |
|---|---|---|
| **Shared** | — | `FrameworkReference: Microsoft.AspNetCore.App` (HttpContext, MVC types, localization, memory cache) |
| **DAL** | Shared | `Microsoft.EntityFrameworkCore` (core only — no SQL Server provider here) |
| **BLL** | DAL, Shared | `AutoMapper.Extensions.Microsoft.DependencyInjection` |
| **PL** | BLL, Shared | `Microsoft.EntityFrameworkCore.SqlServer`, `.Design`, `.Tools`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore` |

The SQL Server provider and EF design-time tooling live only in **PL**, since it's the startup project — `UseSqlServer(...)` is configured there and `dotnet ef` runs against it.

---

## Repository Pattern (important — not generic)

This project deliberately avoids a generic `IRepository<T>` / `Repository<T>`. Instead:

- Every **Definitions** module entity (Employee, Department, Product, ...) gets its own **explicit, non-generic method set** inside a single combined repository: `IDefinitionRepository` / `DefinitionRepository`.

  ```csharp
  Task<IEnumerable<Employee>> GetAllEmployeesAsync();
  Task<Employee?> GetEmployeeByIdAsync(int id);
  Task AddEmployeeAsync(Employee entity);
  Task UpdateEmployeeAsync(Employee entity);
  Task DeleteEmployeeAsync(int id);
  ```

  Reads use `AsNoTracking()`; no reflection, no generic constraints.

- Despite the repository being combined per module group, **each entity still gets its own `IService` / `Service` / `Controller` / Views**, one-to-one — the repository is the only thing that's shared.

- `IUnitOfWork` exposes `DefinitionRepository` (and future combined repositories, e.g. `IReportRepository`) plus a single `SaveChangesAsync()`.

This convention is documented directly in `TradeERP.DAL/IRepositories/IDefinitionRepository.cs`.

---

## How the Application Boots

`Program.cs` is intentionally a thin entry point — all wiring lives in two extension classes under `TradeERP.PL/Extensions/`:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.ConfigureAppSettings();   // WebApplicationBuilderExtensions.cs
var app = builder.Build();
await app.ConfigureRequestPipeline();   // WebApplicationExtensions.cs
app.Run();
```

**`ConfigureAppSettings`** registers, grouped by concern:
- `ApplicationDbContext` (SQL Server, 60s command timeout)
- MVC + JSON options + view localization
- Request localization (English + Arabic cultures)
- ASP.NET Core Identity (cookie auth, role scaffolding)
- Response compression (Brotli/Gzip)
- Session
- `AddBllServices()` (AutoMapper + `IUnitOfWork` + future entity services)
- Form/Kestrel upload size limits

**`ConfigureRequestPipeline`** wires the middleware pipeline in the correct order (response compression *before* static files, so static assets are actually compressed) and, at the end, **calls `dbContext.Database.MigrateAsync()`** — pending EF Core migrations are applied automatically on every startup. Migration failures are caught and logged rather than crashing the app, so the app still comes up if the database isn't reachable yet (useful before any DB is provisioned).

---

## Localization

Strings are resolved from flat JSON files instead of `.resx`:

```
TradeERP.PL/Resources/en.json
TradeERP.PL/Resources/ar.json
```

`IStringLocalizerFactory` is implemented by `JsonStringLocalizerFactory` (`TradeERP.Shared/HelperServices`), backed by an in-memory cache so files aren't re-read on every request. Views support suffix-based localization (`Index.ar.cshtml` next to `Index.cshtml`), and `RequestLocalizationOptions` supports both `en` and `ar` cultures (Arabic configured with Gregorian calendar and Latin digits/decimal separators).

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (local or remote)

### Setup

1. Update the connection string in `TradeERP.PL/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TradeERP;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. Restore and build:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run (migrations apply automatically on startup):
   ```bash
   dotnet run --project TradeERP.PL
   ```

### Adding a migration

Migrations are generated against `TradeERP.DAL` with `TradeERP.PL` as the startup project:

```bash
dotnet ef migrations add <MigrationName> --project TradeERP.DAL --startup-project TradeERP.PL
```

---

## Roadmap

This skeleton is ready for the first real Definitions module (e.g. Employee, Department). Each new module follows the same pattern:

1. Add the entity to `TradeERP.DAL/Models`
2. Add its method group to `IDefinitionRepository` / `DefinitionRepository`
3. Add its ViewModel to `TradeERP.Shared/ViewModels`
4. Add its AutoMapper mapping to `TradeERP.BLL/MappingProfiles`
5. Add its `I{Entity}Service` / `{Entity}Service` to `TradeERP.BLL`
6. Add its `{Entity}Controller` + Views to `TradeERP.PL`
