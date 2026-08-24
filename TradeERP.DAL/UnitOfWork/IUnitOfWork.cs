using TradeERP.DAL.IRepositories;

namespace TradeERP.DAL.UnitOfWork
{
    /// <summary>
    /// Exposes the combined repositories and a single SaveChangesAsync for the
    /// whole request. Future combined repositories (e.g. IReportRepository)
    /// get their own property here, following the same pattern as DefinitionRepository.
    /// </summary>
    public interface IUnitOfWork
    {
        IDefinitionRepository DefinitionRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
