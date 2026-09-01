namespace TradeERP.Shared.HelperServices.Interfaces
{
    public interface IToastrService
    {
        void Success(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
