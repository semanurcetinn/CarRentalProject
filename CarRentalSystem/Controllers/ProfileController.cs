using CarRentalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRentalSystem.Controllers
{
    [Authorize]  // sadece giriş yapmış kullanıcılar erişir
    public class ProfileController : Controller
    {
        CarRentalContext _context = new CarRentalContext();
        public IActionResult Index()
        {
            // giriş yapan kullanıcının ID'sini çerezden okur
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // kullamıcı bilgilerini veritabanından getirir
            var user = _context.Users.Find(currentUserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // kullanıcıya ait rezervasyonları araç bilgileriyle birlikte listeler
            var myReservations = _context.Reservations.Include(r => r.Car).Where(r => r.UserId == currentUserId).OrderByDescending(r => r.StartDate).ToList();

            ViewBag.UserInfo = user;

            return View(myReservations);
        }
    }
}
