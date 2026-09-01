namespace TradeERP.DAL.IRepositories.ICommons
{
    public interface IValidatorRepository<T> where T : class
    {
        bool IsCodeExist(int id, string code);
        bool IsArNameExist(int id, string arName);
        bool IsEnNameExist(int id, string enName);
    }
}
