using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TradeERP.Shared.ViewModels.Commons
{
    /// <summary>
    /// Builds a standard "~/Views/Shared/Error.cshtml" ViewResult from controllers,
    /// without each controller needing to build the ViewDataDictionary boilerplate itself.
    /// </summary>
    public static class ErrorHandler
    {
        public static ViewResult ErrorView(string errorMessage)
            => BuildView(errorMessage);

        public static ViewResult ErrorView(Exception ex)
            => BuildView(ex?.Message ?? "An unexpected error occurred. Please try again later.");

        private static ViewResult BuildView(string errorMessage)
        {
            var model = new ErrorViewModel { ErrorMessage = errorMessage };

            return new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = new ViewDataDictionary<ErrorViewModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                }
            };
        }
    }
}
