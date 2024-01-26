using Microsoft.AspNetCore.Mvc;
using magic_villa_web.Services.IServices;
using magic_villa_web.Models;
using magic_villa_web.Models.Dto;
using magic_villa_utillity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace magic_villa_web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                if (jwt != null && jwt.Claims != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    var nameClaim = jwt.Claims.FirstOrDefault(u => u.Type == "name");
                    if (nameClaim != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
                    }

                    var roleClaim = jwt.Claims.FirstOrDefault(u => u.Type == "role");
                    if (roleClaim != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetString(SD.SessionToken, model.Token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Claims in JWT are null.");
                    return View(obj);
                }
            }
            else
            {
                ModelState.AddModelError("CustomError", response?.ErrorMessages?.FirstOrDefault() ?? "Login failed.");
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);

            if (result !=null && result.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

