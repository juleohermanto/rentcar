using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; // WAJIB ADA INI UNTUK .Include()
using RentCar.Data;
using RentCar.Models;
using System.Linq;

namespace RentCar.Controllers
{
    public class CarController : Controller
    {
        private readonly RentCarContext _context;

        public CarController(RentCarContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? pickupDate, DateTime? returnDate, int? year, string sortOrder)
        {
  
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "harga_rendah";

            ViewBag.SortOrder = sortOrder;
            ViewBag.PickupDate = pickupDate;
            ViewBag.ReturnDate = returnDate;
            ViewBag.Year = year;

            if (pickupDate == null || returnDate == null)
            {
                if (HttpContext.Request.Query.ContainsKey("pickupDate"))
                {
                    TempData["Error"] = " Mohon pilih Tanggal Pickup dan Return terlebih dahulu!";
                }
                return View(new List<MsCar>());
            }
            if (returnDate <= pickupDate)
            {
                TempData["Error"] = "⚠️ Tanggal Kembali harus lebih besar dari Tanggal Pickup!";
                return View(new List<MsCar>());
            }
            var query = _context.MsCars
                                .Include(c => c.MsCarImages)
                                .AsQueryable();
            if (year != null)
            {
                query = query.Where(m => m.year == year);
            }
            switch (sortOrder)
            {
                case "harga_tinggi": query = query.OrderByDescending(m => m.price_per_day); break;
                case "nama_az": query = query.OrderBy(m => m.name); break;
                case "tahun_terbaru": query = query.OrderByDescending(m => m.year); break;
                case "harga_rendah":
                default: query = query.OrderBy(m => m.price_per_day); break;
            }
            return View(query.ToList());
        }
    }
}