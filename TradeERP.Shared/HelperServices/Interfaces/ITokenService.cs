using Microsoft.AspNetCore.Identity;

namespace TradeERP.Shared.HelperServices.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(IdentityUser user, IList<string> roles);
    }
}
