using Microsoft.EntityFrameworkCore;
using TradeERP.DAL.Data;
using TradeERP.DAL.IRepositories.ICommons;

namespace TradeERP.DAL.Repositories.Commons
{
    public class ValidatorRepository<T> : IValidatorRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public ValidatorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsCodeExist(int id, string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            var lowerCode = code.ToLower();

            return _context.Set<T>()
                .Any(e =>
                    EF.Property<string>(e, "Code").ToLower() == lowerCode &&
                    EF.Property<int>(e, "Id") != id);
        }

        public bool IsArNameExist(int id, string arName)
        {
            if (string.IsNullOrEmpty(arName))
                return false;

            var lowerArName = arName.ToLower();

            return _context.Set<T>()
                .Any(e =>
                    EF.Property<string>(e, "ArName").ToLower() == lowerArName &&
                    EF.Property<int>(e, "Id") != id);
        }

        public bool IsEnNameExist(int id, string enName)
        {
            if (string.IsNullOrEmpty(enName))
                return false;

            var lowerEnName = enName.ToLower();

            return _context.Set<T>()
                .Any(e =>
                    EF.Property<string>(e, "EnName").ToLower() == lowerEnName &&
                    EF.Property<int>(e, "Id") != id);
        }
    }
}
