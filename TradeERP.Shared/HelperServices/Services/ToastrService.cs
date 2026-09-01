using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TradeERP.Shared.Constants;
using TradeERP.Shared.Enums;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.Shared.HelperServices.Services
{
    public class ToastrService : IToastrService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public ToastrService(
            IHttpContextAccessor httpContextAccessor,
            ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        private void Notification(ToastrType type, string message)
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available.");
            var tempData = _tempDataDictionaryFactory.GetTempData(context);

            var toastrs = tempData.ContainsKey(ToastrDefaults.ToasterList)
                ? JsonSerializer.Deserialize<List<ToastrData>>(tempData[ToastrDefaults.ToasterList]!.ToString()!) ?? new List<ToastrData>()
                : new List<ToastrData>();

            toastrs.Add(new ToastrData { Message = message, Type = type });

            tempData[ToastrDefaults.ToasterList] = JsonSerializer.Serialize(toastrs);
        }

        public void Success(string message) => Notification(ToastrType.Success, message);
        public void Info(string message) => Notification(ToastrType.Info, message);
        public void Warning(string message) => Notification(ToastrType.Warning, message);
        public void Error(string message) => Notification(ToastrType.Error, message);
    }
}
