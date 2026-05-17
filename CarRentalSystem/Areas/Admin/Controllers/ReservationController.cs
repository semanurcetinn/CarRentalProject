using CarRentalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Formats.Tar;

namespace CarRentalSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        CarRentalContext _context = new CarRentalContext();

        public IActionResult Index()
        {
            // Rezervasyonları, ilişkili Kullanıcı ve Araç verileriyle birlikte tarihe göre azalan sırayla çeker
            var reservations = _context.Reservations.Include(r => r.Car).Include(r => r.User).OrderByDescending(r => r.StartDate).ToList();
            
            return View(reservations);
        }

        // aracı müşteriden teslim alma işlemi
        public IActionResult Complete(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation != null)
            {
                reservation.ReservationStatus = "Tamamlandı";
                _context.Reservations.Update(reservation);

                var car = _context.Cars.Find(reservation.CarId);

                if (car != null)
                {
                    // aracın durumunu tekrar kiralanabilir hale getirir
                    car.CarStatus = "Müsait";
                    _context.Cars.Update(car);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
