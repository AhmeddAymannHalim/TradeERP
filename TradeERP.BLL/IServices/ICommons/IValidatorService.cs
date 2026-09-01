namespace TradeERP.BLL.IServices.ICommons
{
    public interface IValidatorService<T> where T : class
    {
        bool IsCodeExist(int id, string code);
        bool IsArNameExist(int id, string arName);
        bool IsEnNameExist(int id, string enName);
    }
}
