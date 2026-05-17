using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly CarRentalContext _context;

        public CarController(CarRentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        public IActionResult Add()
        {
            // Veritabanındaki en büyük Id değerini bulup 1 artırır. Tablo boşsa 1 atar.
            int newId = _context.Cars.Any() ? _context.Cars.Max(c => c.CarId) + 1 : 1;

            var cars = new Car
            {
                CarId = newId, // Manuel ID ataması
                CarBrand = "BMW",
                CarModel = "M4",
                CarDailyPrice = 3000,
                CarPlate = "55AB27",
                CarStatus = "Available",
                FuelId = 1,
                GearId = 1,
                CategoryId = 1
            };

            _context.Cars.Add(cars);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}