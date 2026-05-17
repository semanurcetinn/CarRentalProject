using CarRentalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        CarRentalContext _context = new CarRentalContext();
        public IActionResult Index()
        {
            ViewBag.TotalRevenue = _context.Payments.Sum(p => p.Amount);
            ViewBag.ActiveReservations = _context.Reservations.Count(r => r.ReservationStatus == "Aktif");
            ViewBag.TotalCustomers = _context.Users.Count(u => u.RoleId == 2);
            ViewBag.AvailableCars = _context.Cars.Count(c => c.CarStatus == "Müsait" && c.IsDeleted == false);

            var recentReservations = _context.Reservations.OrderByDescending(r => r.StartDate).Take(5).ToList();

            return View(recentReservations);
        }
    }
}
