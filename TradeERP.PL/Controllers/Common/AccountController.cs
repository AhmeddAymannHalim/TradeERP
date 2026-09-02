using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.PL.Controllers.Common
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IToastrService _toastr;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IStringLocalizer<SharedResource> localizer,
            IToastrService toastr,
            IEmailService emailService,
            IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
            _toastr = toastr;
            _emailService = emailService;
            _environment = environment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            ViewBag.ExternalProviders = schemes.Select(s => s.Name).ToHashSet();

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = _localizer["ValidationError"].Value });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Json(new { success = false, message = _localizer["InvalidLoginAttempt"].Value });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.IsLockedOut)
                return Json(new { success = false, message = _localizer["AccountLockedOut"].Value });

            if (!result.Succeeded)
                return Json(new { success = false, message = _localizer["InvalidLoginAttempt"].Value });

            return Json(new
            {
                success = true,
                message = _localizer["Welcome"].Value,
                redirectUrl = !string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)
                    ? model.ReturnUrl
                    : Url.Action("Index", "Home")
            });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Json(new { success = true, message = _localizer["SignOut"].Value, redirectUrl = Url.Action("Login", "Account") });
        }

        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Login");
            }

            var model = new AccountSettingsViewModel
            {
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = _localizer["RecordNotFound"].Value });

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault()?.Description ?? _localizer["SomethingWentError"].Value;
                return Json(new { success = false, message });
            }

            await _signInManager.RefreshSignInAsync(user);

            return Json(new { success = true, message = _localizer["PasswordChangedSuccessfully"].Value });
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var user = await _userManager.FindByEmailAsync(model.Email);
            string? devResetLink = null;

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var resetLink = Url.Action("ResetPassword", "Account",
                    new { email = model.Email, token = encodedToken }, Request.Scheme);

                await _emailService.SendAsync(
                    model.Email,
                    _localizer["ResetPasswordEmailSubject"].Value,
                    $"<p>{_localizer["ResetPasswordEmailBody"].Value}</p><p><a href=\"{resetLink}\">{resetLink}</a></p>");

                if (_environment.IsDevelopment() && !_emailService.IsConfigured)
                    devResetLink = resetLink;
            }

            return Json(new
            {
                success = true,
                message = _localizer["ResetLinkSentIfAccountExists"].Value,
                devResetLink
            });
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Json(new { success = false, message = _localizer["InvalidOrExpiredToken"].Value });

            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = _localizer["InvalidOrExpiredToken"].Value });
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.Any(e => e.Code == "InvalidToken")
                    ? _localizer["InvalidOrExpiredToken"].Value
                    : result.Errors.FirstOrDefault()?.Description ?? _localizer["SomethingWentError"].Value;
                return Json(new { success = false, message });
            }

            return Json(new { success = true, message = _localizer["PasswordResetSuccessfully"].Value, redirectUrl = Url.Action("Login") });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl = null)
        {
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            if (!schemes.Any(s => string.Equals(s.Name, provider, StringComparison.OrdinalIgnoreCase)))
            {
                _toastr.Error(_localizer["ExternalLoginError"]);
                return RedirectToAction("Login");
            }

            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                _toastr.Error(_localizer["ExternalLoginError"]);
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _toastr.Error(_localizer["ExternalLoginError"]);
                return RedirectToAction("Login");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return LocalRedirectOrHome(returnUrl);

            if (signInResult.IsLockedOut)
            {
                _toastr.Error(_localizer["AccountLockedOut"]);
                return RedirectToAction("Login");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                _toastr.Error(_localizer["ExternalLoginError"]);
                return RedirectToAction("Login");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _toastr.Error(_localizer["ExternalLoginError"]);
                    return RedirectToAction("Login");
                }

                await _userManager.AddToRoleAsync(user, TradeERP.DAL.SeedData.IdentitySeeder.EmployeeRole);
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                _toastr.Error(_localizer["ExternalLoginError"]);
                return RedirectToAction("Login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirectOrHome(returnUrl);
        }

        private IActionResult LocalRedirectOrHome(string? returnUrl)
        {
            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl)
                : RedirectToAction("Index", "Home");
        }
    }
}
