using Microsoft.AspNetCore.Authorization;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CarRentalSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // admin kısmına sadece adminler girer
    public class CarController : Controller
    {
        CarRentalContext _context = new CarRentalContext();
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Cars.Where(c => c.IsDeleted == false).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Gears = _context.GearTypes.ToList();
            ViewBag.Fuels = _context.FuelsTypes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Car newCar, IFormFile CarImage)
        {
            bool existCarPlate = _context.Cars.Any(x => x.CarPlate == newCar.CarPlate);

            if (CarImage != null && CarImage.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(CarImage.FileName);

                string imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                string savePath = Path.Combine(imageDirectory, fileName);


                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    CarImage.CopyTo(stream);
                }

                newCar.CarImageUrl = "/images/" + fileName;
            }
            else
            {
                newCar.CarImageUrl = "https://via.placeholder.com/400x250/343a40/FFFFFF?text=Gorsel+Yok";
            }

            if (existCarPlate)
            {
                // Hata mesajını arayüze gönderir
                ModelState.AddModelError("CarPlate", "Error: This plate already exist!");

                // Form hata verdiğinde menülerin boş gelmemesi için verileri tekrar gönderir
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Gears = _context.GearTypes.ToList();
                ViewBag.Fuels = _context.FuelsTypes.ToList();

                return View(newCar);  // Kullanıcıyı girdiği verilerle birlikte forma geri döndür
            }
            _context.Cars.Add(newCar);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Kaydedince listeye dön
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Gears = _context.GearTypes.ToList();
            ViewBag.Fuels = _context.FuelsTypes.ToList();

            return View(car);  // düzenlenecek aracın verilerini forma gönderir
        }

        [HttpPost]
        public IActionResult Edit(Car updatedCar, IFormFile? CarImage)
        {
            var existingCar = _context.Cars.Find(updatedCar.CarId);
            if (existingCar == null) return NotFound();

            if (CarImage != null && CarImage.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(CarImage.FileName);
                string imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                string savePath = Path.Combine(imageDirectory, fileName);


                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    CarImage.CopyTo(stream);
                }

                existingCar.CarImageUrl = "/images/" + fileName;
            }
            existingCar.CarPlate = updatedCar.CarPlate;
            existingCar.CarBrand = updatedCar.CarBrand;
            existingCar.CarModel = updatedCar.CarModel;
            existingCar.CarDailyPrice = updatedCar.CarDailyPrice;
            existingCar.CarStatus = updatedCar.CarStatus;
            existingCar.FuelId = updatedCar.FuelId;
            existingCar.GearId = updatedCar.GearId;
            existingCar.CategoryId = updatedCar.CategoryId;

            _context.Cars.Update(existingCar);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                car.IsDeleted = true;
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }  
    }
}