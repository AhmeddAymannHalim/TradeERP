using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TradeERP.DAL.Data;
using TradeERP.Shared;
using TradeERP.Shared.Extensions;
using TradeERP.Shared.HelperServices.Interfaces;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.PL.Controllers.Common
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private const int PageSize = 10;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IToastrService _toastr;

        public UsersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IStringLocalizer<SharedResource> localizer,
            IToastrService toastr)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _localizer = localizer;
            _toastr = toastr;
        }

        public async Task<IActionResult> Index(string? searchString, int pageNo = 1)
        {
            var query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(u => u.Email!.Contains(searchString) || u.UserName!.Contains(searchString));

            query = query.OrderBy(u => u.Email);

            var totalRecords = await query.CountAsync();
            var users = await query.Skip((pageNo - 1) * PageSize).Take(PageSize).ToListAsync();
            var userIds = users.Select(u => u.Id).ToList();

            var rolesByUserId = (await (
                from ur in _db.UserRoles
                join r in _db.Roles on ur.RoleId equals r.Id
                where userIds.Contains(ur.UserId)
                select new { ur.UserId, RoleName = r.Name! }).ToListAsync())
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.RoleName).ToList());

            var now = DateTimeOffset.UtcNow;
            var items = users.Select(user => new UserListItemViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Roles = rolesByUserId.TryGetValue(user.Id, out var roles) ? roles : new List<string>(),
                IsLockedOut = user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value > now
            }).ToList();

            var result = new PaginatedResult<UserListItemViewModel>
            {
                Data = items,
                TotalRecords = totalRecords,
                PageNo = pageNo,
                PageSize = PageSize,
                NoOfPages = (int)Math.Ceiling(totalRecords / (double)PageSize),
                SearchString = searchString
            };

            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateUserViewModel
            {
                AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var message = result.Errors.FirstOrDefault()?.Description ?? _localizer["ErrorWhileSaving"].Value;
                return Json(new { success = false, message });
            }

            var roleResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                var message = roleResult.Errors.FirstOrDefault()?.Description ?? _localizer["ErrorWhileSaving"].Value;
                return Json(new { success = false, message });
            }

            return Json(new
            {
                success = true,
                message = _localizer["SavedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _toastr.Error(_localizer["RecordNotFound"]);
                return RedirectToAction("Index");
            }

            var model = new UpdateUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                SelectedRoles = (await _userManager.GetRolesAsync(user)).ToList(),
                AvailableRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToErrorDictionary(), message = _localizer["ValidationError"].Value });

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return Json(new { success = false, message = _localizer["RecordNotFound"].Value });

            if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal)
                && !model.SelectedRoles.Contains(TradeERP.DAL.SeedData.IdentitySeeder.AdminRole))
            {
                return Json(new { success = false, message = _localizer["CannotModifyOwnAccount"].Value });
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                var message = removeResult.Errors.FirstOrDefault()?.Description ?? _localizer["ErrorWhileUpdating"].Value;
                return Json(new { success = false, message });
            }

            var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!addResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, currentRoles);
                var message = addResult.Errors.FirstOrDefault()?.Description ?? _localizer["ErrorWhileUpdating"].Value;
                return Json(new { success = false, message });
            }

            return Json(new
            {
                success = true,
                message = _localizer["UpdatedSuccessfully"].Value,
                redirectUrl = Url.Action("Index")
            });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLock(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return Json(new { success = false, message = _localizer["RecordNotFound"].Value });

                if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal))
                    return Json(new { success = false, message = _localizer["CannotModifyOwnAccount"].Value });

                var isLocked = await _userManager.IsLockedOutAsync(user);
                await _userManager.SetLockoutEndDateAsync(user, isLocked ? null : DateTimeOffset.MaxValue);

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return Json(new { success = false, message = _localizer["RecordNotFound"].Value });

                if (string.Equals(user.Id, _userManager.GetUserId(User), StringComparison.Ordinal))
                    return Json(new { success = false, message = _localizer["CannotModifyOwnAccount"].Value });

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return Json(new { success = false, message = result.Errors.FirstOrDefault()?.Description ?? _localizer["DeleteFailed"].Value });

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = _localizer["SomethingWentError"].Value });
            }
        }
    }
}
