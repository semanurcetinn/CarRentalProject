using CarRentalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRentalSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        CarRentalContext _context = new CarRentalContext();

        [HttpGet]
        public IActionResult Create(int id)
        {
            var car = _context.Cars.Find(id);

            if (car == null || car.CarStatus != "Müsait")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(car);
        }

        [HttpPost]
        public IActionResult Create(int CarId, DateTime StartDate, DateTime EndDate)
        {
            if (StartDate < DateTime.Today)
            {
                ModelState.AddModelError("", "Başlangıç tarihi bugünden eski olamaz.");
                return View(_context.Cars.Find(CarId));
            }
            
            if (EndDate <= StartDate)
            {
                ModelState.AddModelError("", "Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
                return View(_context.Cars.Find(CarId));
            }

            bool isOverlapping = _context.Reservations.Any(r => r.CarId == CarId && r.ReservationStatus == "Aktif" && StartDate <= r.EndDate && EndDate >= r.StartDate);

            if (isOverlapping)
            {
                ModelState.AddModelError("", "Bu araç seçtiğiniz tarihler arasında başka bir müşteriye kiralıdır. Lütfen farklı tarihler veya farklı bir araç seçin");
                return View(_context.Cars.Find(CarId));
            }

            // URL'den gelen ID'ye sahip aracı veritabanından bulur
            var car = _context.Cars.Find(CarId);

            // araç yoksa ya da kiralanmışsa işlem olmaz
            if (car == null || car.CarStatus != "Müsait")
            {
                return RedirectToAction("Index", "Home");
            }
            
            // maliyet hesaplaması
            TimeSpan dateDifference = EndDate - StartDate;
            int totalDays = dateDifference.Days;

            // araç aynı gün iade edilecekse en az 1 günlük ücret yansıtılır
            if (totalDays == 0) totalDays = 1;

            decimal calculatedTotal = totalDays * car.CarDailyPrice;


            // çerezden giriş yapan kullanıcının ID'sini okur
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Reservation newReservation = new Reservation
            {
                CarId = CarId,
                UserId = currentUserId,
                StartDate = StartDate,
                EndDate = EndDate,
                TotalAmount = calculatedTotal,
                ReservationStatus = "Ödeme Bekliyor"
            };

            // rezervasyonu ekler
            _context.Reservations.Add(newReservation);
            _context.SaveChanges();  // değişiklikleri sqle kaydeder

            return RedirectToAction("Index", "Payment", new {id = newReservation.ReservationId});
        }
    }
}
