using DigitalMarketing.Services.DigitalMarketing.Services.DTOs.AdminUserDtos;
using DigitalMarketing.Services.DigitalMarketing.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalMarketing.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdminAuthService _authService;
        public AccountController(IAdminAuthService authService)
        {
            _authService = authService;
        }




        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");


            return View(new LoginDto());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.ValidateLoginAsync(dto);
            if(!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(dto);
            }



            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.UserName),
                new Claim(ClaimTypes.NameIdentifier, result.Data.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            await _authService.UpdateLastLoginAsync(result.Data);

            return RedirectToAction("Index", "Home");
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
