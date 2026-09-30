using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;
using System.Security.Claims;

namespace RentCar.Controllers
{
    [Authorize] 
    public class BookingController : Controller
    {
        private readonly RentCarContext _context;
        public BookingController(RentCarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(string carId, DateTime pickupDate, DateTime returnDate, string sortOrder)
        {
            var mobil = _context.MsCars
                                .Include(c => c.MsCarImages)
                                .FirstOrDefault(c => c.Car_id == carId);
            if (mobil == null) return NotFound();
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "harga_rendah";
            }

            IQueryable<MsCar> queryUrutan = _context.MsCars;

            switch (sortOrder)
            {
                case "harga_tinggi":
                    queryUrutan = queryUrutan.OrderByDescending(c => c.price_per_day);
                    break;
                case "nama_az":
                    queryUrutan = queryUrutan.OrderBy(c => c.name);
                    break;
                case "tahun_terbaru":
                    queryUrutan = queryUrutan.OrderByDescending(c => c.year);
                    break;
                case "harga_rendah":
                default: 
                    queryUrutan = queryUrutan.OrderBy(c => c.price_per_day);
                    break;
            }

            var allCars = queryUrutan.Select(c => c.Car_id).ToList();

            int currentIndex = allCars.IndexOf(carId);

            if (currentIndex > 0)
                ViewBag.PrevCarId = allCars[currentIndex - 1];

            if (currentIndex < allCars.Count - 1)
                ViewBag.NextCarId = allCars[currentIndex + 1];

            ViewBag.PickupDate = pickupDate;
            ViewBag.ReturnDate = returnDate;
            ViewBag.SortOrder = sortOrder; 

            int totalHari = (returnDate - pickupDate).Days;
            if (totalHari < 1) totalHari = 1;
            decimal totalHarga = totalHari * mobil.price_per_day;

            var model = new TrRental
            {
                car_id = carId,
                MsCar = mobil,
                rental_date = pickupDate,
                return_date = returnDate,
                total_price = totalHarga
            };

            ViewBag.TotalHari = totalHari;
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(TrRental model)
        {

            model.Rental_id = Guid.NewGuid().ToString(); 
            model.payment_status = false; 
            model.customer_id = User.FindFirstValue(ClaimTypes.NameIdentifier); 


            if (model.customer_id == null) return RedirectToAction("Login", "Account");

            _context.TrRentals.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Sewa Berhasil! Cek Riwayat Anda.";
            return RedirectToAction("History");
        }

        public IActionResult History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var history = _context.TrRentals
                                  .Include(r => r.MsCar)
                                  .Where(r => r.customer_id == userId)
                                  .OrderByDescending(r => r.rental_date)
                                  .ToList();

            return View(history);
        }

    }
}