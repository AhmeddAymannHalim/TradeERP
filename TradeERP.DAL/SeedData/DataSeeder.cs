using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.Models;
using TradeERP.DAL.Repositories.Definitions;
using TradeERP.Shared.Constants;
using TradeERP.Shared.Enums;

namespace TradeERP.DAL.SeedData
{
    public static class DataSeeder
    {
        private const string SeederUser = "Seeder";

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCountries(context);
            await SeedGovs(context);
            await SeedTowns(context);
            await SeedVillages(context);

            await SeedDepartments(context);
            await SeedSpecializations(context);
            await SeedEmployees(context);
            await SeedCategories(context);
            await SeedProducts(context);

            await SeedNumberingSettings(context);
            await SeedSystemLedgerAccounts(context);
            await SeedChartOfAccounts(context);
            await SeedAccountingPeriod(context);

            await SeedCustomers(context);
            await SeedSuppliers(context);
            await LinkPartyLedgerAccounts(context);

            await SeedSampleTransactions(context);
        }

        private static async Task SeedDepartments(ApplicationDbContext context)
        {
            var existingCodes = await context.Departments.Select(d => d.Code).ToListAsync();

            context.Departments.AddRange(new[]
            {
                new Department { Code = "DEP-01", ArName = "المبيعات", EnName = "Sales", CreatedBy = SeederUser },
                new Department { Code = "DEP-02", ArName = "المشتريات", EnName = "Purchasing", CreatedBy = SeederUser },
                new Department { Code = "DEP-03", ArName = "المخازن", EnName = "Warehouse", CreatedBy = SeederUser },
                new Department { Code = "DEP-04", ArName = "الحسابات", EnName = "Accounting", CreatedBy = SeederUser }
            }.Where(d => !existingCodes.Contains(d.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedSpecializations(ApplicationDbContext context)
        {
            var existingCodes = await context.Specializations.Select(s => s.Code).ToListAsync();

            context.Specializations.AddRange(new[]
            {
                new Specialization { Code = "SPC-01", ArName = "مندوب مبيعات", EnName = "Sales Representative", CreatedBy = SeederUser },
                new Specialization { Code = "SPC-02", ArName = "أمين مخزن", EnName = "Warehouse Keeper", CreatedBy = SeederUser },
                new Specialization { Code = "SPC-03", ArName = "محاسب", EnName = "Accountant", CreatedBy = SeederUser }
            }.Where(s => !existingCodes.Contains(s.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedEmployees(ApplicationDbContext context)
        {
            var existingCodes = await context.Employees.Select(e => e.Code).ToListAsync();
            if (existingCodes.Contains("EMP-01") && existingCodes.Contains("EMP-02") && existingCodes.Contains("EMP-03"))
                return;

            var salesDept = await context.Departments.FirstAsync(d => d.Code == "DEP-01");
            var purchasingDept = await context.Departments.FirstAsync(d => d.Code == "DEP-02");
            var warehouseDept = await context.Departments.FirstAsync(d => d.Code == "DEP-03");

            var salesSpec = await context.Specializations.FirstAsync(s => s.Code == "SPC-01");
            var warehouseSpec = await context.Specializations.FirstAsync(s => s.Code == "SPC-02");
            var accountingSpec = await context.Specializations.FirstAsync(s => s.Code == "SPC-03");

            context.Employees.AddRange(new[]
            {
                new Employee
                {
                    Code = "EMP-01",
                    ArName = "أحمد أيمن",
                    EnName = "Ahmed Ayman",
                    Address = "مدينة نصر، القاهرة",
                    PhoneNumber = "01000000001",
                    NationalId = "29001010100011",
                    Email = "ahmed.sales@tradeerp.local",
                    HireDate = new DateTime(2023, 1, 15),
                    BirthDate = new DateTime(1998, 3, 20),
                    Gender = Gender.Male,
                    MaritalStatus = MaritalStatus.Single,
                    ContractType = ContractType.Permanent,
                    JobTitle = "Sales Representative",
                    BasicSalary = 8000,
                    DepartmentId = salesDept.Id,
                    SpecializationId = salesSpec.Id,
                    CreatedBy = SeederUser
                },
                new Employee
                {
                    Code = "EMP-02",
                    ArName = "منى سامي",
                    EnName = "Mona Sami",
                    Address = "الدقي، الجيزة",
                    PhoneNumber = "01000000002",
                    NationalId = "29202020200022",
                    Email = "mona.wh@tradeerp.local",
                    HireDate = new DateTime(2022, 6, 1),
                    BirthDate = new DateTime(1995, 7, 11),
                    Gender = Gender.Female,
                    MaritalStatus = MaritalStatus.Married,
                    ContractType = ContractType.Permanent,
                    JobTitle = "Warehouse Keeper",
                    BasicSalary = 7000,
                    DepartmentId = warehouseDept.Id,
                    SpecializationId = warehouseSpec.Id,
                    CreatedBy = SeederUser
                },
                new Employee
                {
                    Code = "EMP-03",
                    ArName = "كريم حسن",
                    EnName = "Karim Hassan",
                    Address = "المعادي، القاهرة",
                    PhoneNumber = "01000000003",
                    NationalId = "29303030300033",
                    Email = "karim.acc@tradeerp.local",
                    HireDate = new DateTime(2021, 9, 1),
                    BirthDate = new DateTime(1990, 12, 5),
                    Gender = Gender.Male,
                    MaritalStatus = MaritalStatus.Married,
                    ContractType = ContractType.Permanent,
                    JobTitle = "Accountant",
                    BasicSalary = 9500,
                    DepartmentId = purchasingDept.Id,
                    SpecializationId = accountingSpec.Id,
                    CreatedBy = SeederUser
                }
            }.Where(e => !existingCodes.Contains(e.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            var existingCodes = await context.Categories.Select(c => c.Code).ToListAsync();

            context.Categories.AddRange(new[]
            {
                new Category { Code = "CAT-01", ArName = "مواد غذائية", EnName = "Groceries", CreatedBy = SeederUser },
                new Category { Code = "CAT-02", ArName = "أدوات منزلية", EnName = "Home Supplies", CreatedBy = SeederUser },
                new Category { Code = "CAT-03", ArName = "مشروبات", EnName = "Beverages", CreatedBy = SeederUser }
            }.Where(c => !existingCodes.Contains(c.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedProducts(ApplicationDbContext context)
        {
            var existingCodes = await context.Products.Select(p => p.Code).ToListAsync();

            var groceries = await context.Categories.FirstAsync(c => c.Code == "CAT-01");
            var homeSupplies = await context.Categories.FirstAsync(c => c.Code == "CAT-02");
            var beverages = await context.Categories.FirstAsync(c => c.Code == "CAT-03");

            context.Products.AddRange(new[]
            {
                new Product { Code = "PRD-01", ArName = "أرز", EnName = "Rice", CategoryId = groceries.Id, Unit = ProductUnit.Kilogram, Price = 45, CreatedBy = SeederUser },
                new Product { Code = "PRD-02", ArName = "سكر", EnName = "Sugar", CategoryId = groceries.Id, Unit = ProductUnit.Kilogram, Price = 38, CreatedBy = SeederUser },
                new Product { Code = "PRD-03", ArName = "زيت طبخ", EnName = "Cooking Oil", CategoryId = groceries.Id, Unit = ProductUnit.Liter, Price = 90, CreatedBy = SeederUser },
                new Product { Code = "PRD-04", ArName = "منظف أرضيات", EnName = "Floor Cleaner", CategoryId = homeSupplies.Id, Unit = ProductUnit.Piece, Price = 55, CreatedBy = SeederUser },
                new Product { Code = "PRD-05", ArName = "مناديل ورقية", EnName = "Paper Tissues", CategoryId = homeSupplies.Id, Unit = ProductUnit.Carton, Price = 120, CreatedBy = SeederUser },
                new Product { Code = "PRD-06", ArName = "مياه معدنية", EnName = "Mineral Water", CategoryId = beverages.Id, Unit = ProductUnit.Carton, Price = 60, CreatedBy = SeederUser },
                new Product { Code = "PRD-07", ArName = "عصير برتقال", EnName = "Orange Juice", CategoryId = beverages.Id, Unit = ProductUnit.Box, Price = 75, CreatedBy = SeederUser }
            }.Where(p => !existingCodes.Contains(p.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedCustomers(ApplicationDbContext context)
        {
            var existingCodes = await context.Customers.Select(c => c.Code).ToListAsync();

            context.Customers.AddRange(new[]
            {
                new Customer { Code = "CUS-01", ArName = "سوبر ماركت النور", EnName = "El Nour Supermarket", Phone = "0221234567", Address = "مدينة نصر، القاهرة", CreatedBy = SeederUser },
                new Customer { Code = "CUS-02", ArName = "بقالة السلام", EnName = "El Salam Grocery", Phone = "0227654321", Address = "المهندسين، الجيزة", CreatedBy = SeederUser },
                new Customer { Code = "CUS-03", ArName = "مينى ماركت الأمل", EnName = "El Amal Mini Market", Phone = "0233445566", Address = "المعادي، القاهرة", CreatedBy = SeederUser }
            }.Where(c => !existingCodes.Contains(c.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedSuppliers(ApplicationDbContext context)
        {
            var existingCodes = await context.Suppliers.Select(s => s.Code).ToListAsync();

            context.Suppliers.AddRange(new[]
            {
                new Supplier { Code = "SUP-01", ArName = "شركة الدلتا للتجارة", EnName = "Delta Trading Co.", Phone = "035678901", Address = "طنطا، الغربية", CreatedBy = SeederUser },
                new Supplier { Code = "SUP-02", ArName = "مصنع النيل للمشروبات", EnName = "Nile Beverages Factory", Phone = "0221122334", Address = "العاشر من رمضان", CreatedBy = SeederUser },
                new Supplier { Code = "SUP-03", ArName = "شركة الأهرام للمواد الغذائية", EnName = "Al Ahram Foodstuff Co.", Phone = "0223344556", Address = "6 أكتوبر، الجيزة", CreatedBy = SeederUser }
            }.Where(s => !existingCodes.Contains(s.Code)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedNumberingSettings(ApplicationDbContext context)
        {
            if (!await context.BillSettings.AnyAsync())
            {
                context.BillSettings.Add(new BillSetting { Code = "1", Prefix = "INV-", NextNumber = 1 });
            }

            if (!await context.EntrySettings.AnyAsync())
            {
                context.EntrySettings.Add(new EntrySetting { Code = "1", Prefix = "JE-", NextNumber = 1 });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedSystemLedgerAccounts(ApplicationDbContext context)
        {
            var existingCodes = await context.LedgerAccounts
                .Where(a => a.Code == SystemLedgerAccounts.SalesRevenue
                    || a.Code == SystemLedgerAccounts.PurchaseExpense
                    || a.Code == SystemLedgerAccounts.OpeningBalanceEquity)
                .Select(a => a.Code)
                .ToListAsync();

            if (!existingCodes.Contains(SystemLedgerAccounts.SalesRevenue))
            {
                context.LedgerAccounts.Add(new LedgerAccount
                {
                    Code = SystemLedgerAccounts.SalesRevenue,
                    ArName = "إيرادات المبيعات",
                    EnName = "Sales Revenue",
                    AccountType = AccountType.Revenue
                });
            }

            if (!existingCodes.Contains(SystemLedgerAccounts.PurchaseExpense))
            {
                context.LedgerAccounts.Add(new LedgerAccount
                {
                    Code = SystemLedgerAccounts.PurchaseExpense,
                    ArName = "مصروفات المشتريات",
                    EnName = "Purchase Expense",
                    AccountType = AccountType.Expense
                });
            }

            if (!existingCodes.Contains(SystemLedgerAccounts.OpeningBalanceEquity))
            {
                context.LedgerAccounts.Add(new LedgerAccount
                {
                    Code = SystemLedgerAccounts.OpeningBalanceEquity,
                    ArName = "الأرصدة الافتتاحية",
                    EnName = "Opening Balance Equity",
                    AccountType = AccountType.Equity
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedChartOfAccounts(ApplicationDbContext context)
        {
            var existingCodes = await context.LedgerAccounts.Select(a => a.Code).ToListAsync();

            var accounts = new List<LedgerAccount>
            {
                new() { Code = "CASH-01", ArName = "الخزينة", EnName = "Cash Treasury", AccountType = AccountType.Asset }
            };

            foreach (var customerCode in new[] { "CUS-01", "CUS-02", "CUS-03" })
            {
                accounts.Add(new LedgerAccount
                {
                    Code = $"AR-{customerCode}",
                    ArName = $"ذمم عميل {customerCode}",
                    EnName = $"Receivable - {customerCode}",
                    AccountType = AccountType.Asset
                });
            }

            foreach (var supplierCode in new[] { "SUP-01", "SUP-02", "SUP-03" })
            {
                accounts.Add(new LedgerAccount
                {
                    Code = $"AP-{supplierCode}",
                    ArName = $"ذمم مورد {supplierCode}",
                    EnName = $"Payable - {supplierCode}",
                    AccountType = AccountType.Liability
                });
            }

            context.LedgerAccounts.AddRange(accounts.Where(a => !existingCodes.Contains(a.Code)));
            await context.SaveChangesAsync();
        }

        private static async Task LinkPartyLedgerAccounts(ApplicationDbContext context)
        {
            var customers = await context.Customers.Where(c => c.LedgerAccountId == null).ToListAsync();
            foreach (var customer in customers)
            {
                var account = await context.LedgerAccounts.FirstOrDefaultAsync(a => a.Code == $"AR-{customer.Code}");
                if (account != null)
                    customer.LedgerAccountId = account.Id;
            }

            var suppliers = await context.Suppliers.Where(s => s.LedgerAccountId == null).ToListAsync();
            foreach (var supplier in suppliers)
            {
                var account = await context.LedgerAccounts.FirstOrDefaultAsync(a => a.Code == $"AP-{supplier.Code}");
                if (account != null)
                    supplier.LedgerAccountId = account.Id;
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedAccountingPeriod(ApplicationDbContext context)
        {
            var year = DateTime.UtcNow.Year;
            var periodName = $"FY-{year}";

            if (await context.AccountingPeriods.AnyAsync(p => p.Name == periodName))
                return;

            context.AccountingPeriods.Add(new AccountingPeriod
            {
                Name = periodName,
                StartDate = new DateTime(year, 1, 1),
                EndDate = new DateTime(year, 12, 31),
                IsClosed = false
            });

            await context.SaveChangesAsync();
        }

        private static async Task SeedSampleTransactions(ApplicationDbContext context)
        {
            if (!await context.BillMasters.AnyAsync())
            {
                var supplier = await context.Suppliers.FirstAsync(s => s.Code == "SUP-01");
                var rice = await context.Products.FirstAsync(p => p.Code == "PRD-01");
                var sugar = await context.Products.FirstAsync(p => p.Code == "PRD-02");

                var billRepo = new BillMasterRepository(context);

                var purchaseBill = new BillMaster
                {
                    BillDate = DateTime.UtcNow.Date,
                    BillType = BillType.Purchase,
                    SupplierId = supplier.Id
                };
                var purchaseLines = new List<BillDetails>
                {
                    new() { ProductId = rice.Id, Quantity = 100, UnitPrice = rice.Price },
                    new() { ProductId = sugar.Id, Quantity = 100, UnitPrice = sugar.Price }
                };
                await billRepo.AddWithDetailsAndPostAsync(purchaseBill, purchaseLines);

                var customer = await context.Customers.FirstAsync(c => c.Code == "CUS-01");
                var salesBill = new BillMaster
                {
                    BillDate = DateTime.UtcNow.Date,
                    BillType = BillType.Sales,
                    CustomerId = customer.Id
                };
                var salesLines = new List<BillDetails>
                {
                    new() { ProductId = rice.Id, Quantity = 10, UnitPrice = 55 },
                    new() { ProductId = sugar.Id, Quantity = 5, UnitPrice = 45 }
                };
                await billRepo.AddWithDetailsAndPostAsync(salesBill, salesLines);
            }

            if (!await context.VoucherMasters.AnyAsync())
            {
                var treasury = await context.LedgerAccounts.FirstAsync(a => a.Code == "CASH-01");
                var customer = await context.Customers.FirstAsync(c => c.Code == "CUS-01");
                var supplier = await context.Suppliers.FirstAsync(s => s.Code == "SUP-01");

                var voucherRepo = new VoucherMasterRepository(context);

                await voucherRepo.AddAndPostAsync(new VoucherMaster
                {
                    VoucherDate = DateTime.UtcNow.Date,
                    VoucherType = VoucherType.Receipt,
                    CustomerId = customer.Id,
                    TreasuryLedgerAccountId = treasury.Id,
                    Amount = 300,
                    Notes = "Sample receipt seeded for local development"
                });

                await voucherRepo.AddAndPostAsync(new VoucherMaster
                {
                    VoucherDate = DateTime.UtcNow.Date,
                    VoucherType = VoucherType.Payment,
                    SupplierId = supplier.Id,
                    TreasuryLedgerAccountId = treasury.Id,
                    Amount = 500,
                    Notes = "Sample payment seeded for local development"
                });
            }
        }

        private static async Task SeedCountries(ApplicationDbContext context)
        {
            var existingNames = await context.Countries.Select(c => c.EnName).ToListAsync();

            context.Countries.AddRange(new[]
            {
                new Country { ArName = "مصر", EnName = "Egypt" },
                new Country { ArName = "السعودية", EnName = "Saudi Arabia" }
            }.Where(c => !existingNames.Contains(c.EnName)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedGovs(ApplicationDbContext context)
        {
            var egypt = await context.Countries.FirstOrDefaultAsync(c => c.EnName == "Egypt");
            var saudi = await context.Countries.FirstOrDefaultAsync(c => c.EnName == "Saudi Arabia");
            if (egypt == null || saudi == null)
                return;

            var existingNames = await context.Governorates.Select(g => g.EnName).ToListAsync();

            context.Governorates.AddRange(new[]
            {
                new Governorate { CountryId = egypt.Id, ArName = "القاهرة", EnName = "Cairo" },
                new Governorate { CountryId = egypt.Id, ArName = "الجيزة", EnName = "Giza" },
                new Governorate { CountryId = egypt.Id, ArName = "الإسكندرية", EnName = "Alexandria" },
                new Governorate { CountryId = egypt.Id, ArName = "الدقهلية", EnName = "Dakahlia" },
                new Governorate { CountryId = egypt.Id, ArName = "الغربية", EnName = "Gharbia" },

                new Governorate { CountryId = saudi.Id, ArName = "الرياض", EnName = "Riyadh" },
                new Governorate { CountryId = saudi.Id, ArName = "مكة المكرمة", EnName = "Makkah" },
                new Governorate { CountryId = saudi.Id, ArName = "المدينة المنورة", EnName = "Madinah" },
                new Governorate { CountryId = saudi.Id, ArName = "المنطقة الشرقية", EnName = "Eastern Province" }
            }.Where(g => !existingNames.Contains(g.EnName)));

            await context.SaveChangesAsync();
        }

        private static async Task SeedTowns(ApplicationDbContext context)
        {
            var govs = await context.Governorates.ToDictionaryAsync(g => g.EnName, g => g.Id);
            var existingNames = await context.Towns.Select(t => t.EnName).ToListAsync();

            var towns = new List<Town>();

            void AddTown(string govName, string arName, string enName)
            {
                if (govs.TryGetValue(govName, out var govId) && !existingNames.Contains(enName))
                    towns.Add(new Town { GovernorateId = govId, ArName = arName, EnName = enName });
            }

            AddTown("Cairo", "القاهرة", "Cairo");
            AddTown("Cairo", "حلوان", "Helwan");
            AddTown("Giza", "الجيزة", "Giza");
            AddTown("Giza", "6 أكتوبر", "6th of October");
            AddTown("Alexandria", "الإسكندرية", "Alexandria");
            AddTown("Alexandria", "برج العرب", "Borg El Arab");
            AddTown("Dakahlia", "المنصورة", "Mansoura");
            AddTown("Dakahlia", "ميت غمر", "Mit Ghamr");
            AddTown("Gharbia", "طنطا", "Tanta");
            AddTown("Gharbia", "المحلة الكبرى", "El Mahalla El Kubra");
            AddTown("Riyadh", "الرياض", "Riyadh");
            AddTown("Riyadh", "الخرج", "Al Kharj");
            AddTown("Makkah", "مكة المكرمة", "Makkah");
            AddTown("Makkah", "جدة", "Jeddah");
            AddTown("Madinah", "المدينة المنورة", "Madinah");
            AddTown("Madinah", "ينبع", "Yanbu");
            AddTown("Eastern Province", "الدمام", "Dammam");
            AddTown("Eastern Province", "الخبر", "Khobar");

            context.Towns.AddRange(towns);
            await context.SaveChangesAsync();
        }

        private static async Task SeedVillages(ApplicationDbContext context)
        {
            var towns = await context.Towns.ToDictionaryAsync(t => t.EnName, t => t.Id);
            var existingNames = await context.Villages.Select(v => v.EnName).ToListAsync();

            var villages = new List<Village>();

            void AddVillage(string townName, string arName, string enName)
            {
                if (towns.TryGetValue(townName, out var townId) && !existingNames.Contains(enName))
                    villages.Add(new Village { TownId = townId, ArName = arName, EnName = enName });
            }

            AddVillage("Cairo", "مدينة نصر", "Nasr City");
            AddVillage("Cairo", "المعادي", "Maadi");
            AddVillage("Helwan", "التبين", "El Tebbin");
            AddVillage("Giza", "الدقي", "Dokki");
            AddVillage("Giza", "العجوزة", "Agouza");
            AddVillage("6th of October", "الحي الأول", "First District");
            AddVillage("Mansoura", "توريل", "Toriel");
            AddVillage("Riyadh", "العليا", "Al Olaya");
            AddVillage("Riyadh", "الملز", "Al Malaz");
            AddVillage("Jeddah", "الروضة", "Al Rawdah");

            context.Villages.AddRange(villages);
            await context.SaveChangesAsync();
        }
    }
}
