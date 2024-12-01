using AutoTelematicSystem.Dtos.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoTelematicSystem.Controllers
{
    public class AccountController : Controller
    {
        //private readonly 

        public AccountController()
        {

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginDto loginDto)
        {
            if (loginDto.Username == "test" && loginDto.Password == "test")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginDto.Username)
                };
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(principal);

                if (loginDto.RememberMe)
                {
                }

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
