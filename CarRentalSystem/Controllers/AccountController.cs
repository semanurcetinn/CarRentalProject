using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using CarRentalSystem.Models;
using System.Security.Claims;

namespace CarRentalSystem.Controllers
{
    public class AccountController : Controller
    {
        CarRentalContext _context = new CarRentalContext();

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            // e-posta kontrolü
            bool haveEmail = _context.Users.Any(u => u.UserEmail == newUser.UserEmail);

            if (haveEmail)
            {
                ModelState.AddModelError("UserMail", "Bu e-posta adresi sistemde zaten kayıtlı.");
                return View(newUser);
            }

            newUser.RoleId = 2;  // yeni kayıt olan herkes varsayılan olarak customer sayılır

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UserEmail, string UserPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserEmail == UserEmail && u.UserPassword.ToString() == UserPassword);

            if (user != null)
            {
                var userRole = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Role, userRole != null ? userRole.RoleName ?? "Customer" : "Customer")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync("CookieAuth", principal);  //çerez oluşturarak sisteme giriş yapılıyor

                // rolüne göre yönlendirme yapar
                if (userRole != null && userRole.RoleName == "Admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}
