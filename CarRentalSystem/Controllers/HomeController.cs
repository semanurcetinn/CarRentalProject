using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Timers;

namespace CarRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        CarRentalContext _context = new CarRentalContext();

        // parametreler http get isteğinden (url'den) gelir
        public IActionResult Index(int? categoryId, int? gearId)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Gears = _context.GearTypes.ToList();

            // Sadece 'Müsait' araçları getir ve ilişkili tabloları(Kategori, Vites) dahil et
            var query = _context.Cars.Where(c => c.CarStatus == "Müsait" && c.IsDeleted == false).AsQueryable();

            // Kategori filtresi uygulandıysa sorguya ekle
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }

            // Vites filtresi uygulandıysa sorguya ekle
            if (gearId.HasValue && gearId.Value > 0)
            {
                query = query.Where(c => c.GearId == gearId.Value);
            }

            // Oluşturulan dinamik sorguyu veritabanında çalıştır ve listeye çevir
            var filteredCars = query.ToList();

            return View(filteredCars);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
