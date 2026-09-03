using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.Models;
using TradeERP.Shared.Constants;

namespace TradeERP.DAL.SeedData
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCountries(context);
            await SeedGovs(context);
            await SeedTowns(context);
            await SeedVillages(context);
            await SeedSystemLedgerAccounts(context);
            await SeedNumberingSettings(context);
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
                    AccountType = "Revenue"
                });
            }

            if (!existingCodes.Contains(SystemLedgerAccounts.PurchaseExpense))
            {
                context.LedgerAccounts.Add(new LedgerAccount
                {
                    Code = SystemLedgerAccounts.PurchaseExpense,
                    ArName = "مصروفات المشتريات",
                    EnName = "Purchase Expense",
                    AccountType = "Expense"
                });
            }

            if (!existingCodes.Contains(SystemLedgerAccounts.OpeningBalanceEquity))
            {
                context.LedgerAccounts.Add(new LedgerAccount
                {
                    Code = SystemLedgerAccounts.OpeningBalanceEquity,
                    ArName = "الأرصدة الافتتاحية",
                    EnName = "Opening Balance Equity",
                    AccountType = "Equity"
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedCountries(ApplicationDbContext context)
        {
            if (await context.Countries.AnyAsync())
                return;

            context.Countries.AddRange(
                new Country { ArName = "مصر", EnName = "Egypt" },
                new Country { ArName = "السعودية", EnName = "Saudi Arabia" }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedGovs(ApplicationDbContext context)
        {
            if (await context.Governorates.AnyAsync())
                return;

            context.Governorates.AddRange(
                // Egypt (CountryId = 1)
                new Governorate { CountryId = 1, ArName = "القاهرة", EnName = "Cairo" },
                new Governorate { CountryId = 1, ArName = "الجيزة", EnName = "Giza" },
                new Governorate { CountryId = 1, ArName = "الإسكندرية", EnName = "Alexandria" },
                new Governorate { CountryId = 1, ArName = "الدقهلية", EnName = "Dakahlia" },
                new Governorate { CountryId = 1, ArName = "الغربية", EnName = "Gharbia" },

                // Saudi Arabia (CountryId = 2)
                new Governorate { CountryId = 2, ArName = "الرياض", EnName = "Riyadh" },
                new Governorate { CountryId = 2, ArName = "مكة المكرمة", EnName = "Makkah" },
                new Governorate { CountryId = 2, ArName = "المدينة المنورة", EnName = "Madinah" },
                new Governorate { CountryId = 2, ArName = "المنطقة الشرقية", EnName = "Eastern Province" }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedTowns(ApplicationDbContext context)
        {
            if (await context.Towns.AnyAsync())
                return;

            context.Towns.AddRange(
                // Cairo (GovernorateId = 1)
                new Town { GovernorateId = 1, ArName = "القاهرة", EnName = "Cairo" },
                new Town { GovernorateId = 1, ArName = "حلوان", EnName = "Helwan" },

                // Giza (GovernorateId = 2)
                new Town { GovernorateId = 2, ArName = "الجيزة", EnName = "Giza" },
                new Town { GovernorateId = 2, ArName = "6 أكتوبر", EnName = "6th of October" },

                // Alexandria (GovernorateId = 3)
                new Town { GovernorateId = 3, ArName = "الإسكندرية", EnName = "Alexandria" },
                new Town { GovernorateId = 3, ArName = "برج العرب", EnName = "Borg El Arab" },

                // Dakahlia (GovernorateId = 4)
                new Town { GovernorateId = 4, ArName = "المنصورة", EnName = "Mansoura" },
                new Town { GovernorateId = 4, ArName = "ميت غمر", EnName = "Mit Ghamr" },

                // Gharbia (GovernorateId = 5)
                new Town { GovernorateId = 5, ArName = "طنطا", EnName = "Tanta" },
                new Town { GovernorateId = 5, ArName = "المحلة الكبرى", EnName = "El Mahalla El Kubra" },

                // Riyadh (GovernorateId = 6)
                new Town { GovernorateId = 6, ArName = "الرياض", EnName = "Riyadh" },
                new Town { GovernorateId = 6, ArName = "الخرج", EnName = "Al Kharj" },

                // Makkah (GovernorateId = 7)
                new Town { GovernorateId = 7, ArName = "مكة المكرمة", EnName = "Makkah" },
                new Town { GovernorateId = 7, ArName = "جدة", EnName = "Jeddah" },

                // Madinah (GovernorateId = 8)
                new Town { GovernorateId = 8, ArName = "المدينة المنورة", EnName = "Madinah" },
                new Town { GovernorateId = 8, ArName = "ينبع", EnName = "Yanbu" },

                // Eastern Province (GovernorateId = 9)
                new Town { GovernorateId = 9, ArName = "الدمام", EnName = "Dammam" },
                new Town { GovernorateId = 9, ArName = "الخبر", EnName = "Khobar" }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedVillages(ApplicationDbContext context)
        {
            if (await context.Villages.AnyAsync())
                return;

            context.Villages.AddRange(
                // Cairo town (TownId = 1)
                new Village { TownId = 1, ArName = "مدينة نصر", EnName = "Nasr City" },
                new Village { TownId = 1, ArName = "المعادي", EnName = "Maadi" },

                // Helwan town (TownId = 2)
                new Village { TownId = 2, ArName = "التبين", EnName = "El Tebbin" },

                // Giza town (TownId = 3)
                new Village { TownId = 3, ArName = "الدقي", EnName = "Dokki" },
                new Village { TownId = 3, ArName = "العجوزة", EnName = "Agouza" },

                // 6th of October town (TownId = 4)
                new Village { TownId = 4, ArName = "الحي الأول", EnName = "First District" },

                // Mansoura town (TownId = 7)
                new Village { TownId = 7, ArName = "توريل", EnName = "Toriel" },

                // Riyadh town (TownId = 11)
                new Village { TownId = 11, ArName = "العليا", EnName = "Al Olaya" },
                new Village { TownId = 11, ArName = "الملز", EnName = "Al Malaz" },

                // Jeddah town (TownId = 14)
                new Village { TownId = 14, ArName = "الروضة", EnName = "Al Rawdah" }
            );

            await context.SaveChangesAsync();
        }
    }
}
