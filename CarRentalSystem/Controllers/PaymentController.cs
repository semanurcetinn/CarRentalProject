using CarRentalSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        CarRentalContext _context = new CarRentalContext();

        [HttpGet]
        public IActionResult Index(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation == null || reservation.ReservationStatus != "Ödeme Bekliyor")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(reservation);
        }

        [HttpPost]
        public IActionResult Process(int ReservationId)
        {
            var reservation = _context.Reservations.Find(ReservationId);

            if (reservation == null || reservation.ReservationStatus != "Ödeme Bekliyor")
            {
                return RedirectToAction("Index", "Home");
            }

            var car = _context.Cars.Find(reservation.CarId);

            Payment newPayment = new Payment
            {
                ReservationId = reservation.ReservationId,
                Amount = reservation.TotalAmount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(newPayment);

            reservation.ReservationStatus = "Aktif";
            _context.Reservations.Update(reservation);

            if (car != null)
            {
                car.CarStatus = "Kirada";
                _context.Cars.Update(car);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Profile");  // tüm işlemler başarılı olduğunda kullanıcıyı kendi geçmişini göreceği sayfaya gönderir.
        }
    }
}
