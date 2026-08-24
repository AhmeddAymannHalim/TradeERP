using TradeERP.DAL.IRepositories;

namespace TradeERP.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDefinitionRepository DefinitionRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
