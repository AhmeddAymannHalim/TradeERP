using TradeERP.DAL.IRepositories.ICommons;
using TradeERP.DAL.IRepositories.Definitions;

namespace TradeERP.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDefinitionRepository Definitions { get; }
        ILookupRepository Lookups { get; }

        Task<int> SaveChangesAsync();
    }
}
