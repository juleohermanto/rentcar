using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RentCar.Data;
using RentCar.Models;
using System.Security.Claims;

namespace RentCar.Controllers
{
    public class AccountController : Controller
    {
        private readonly RentCarContext _context;

        public AccountController(RentCarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(MsCustomer newCustomer, string rePassword)
        {
            if (newCustomer.password != rePassword)
            {
                ViewBag.Error = "Password dan Re-Password tidak sama!";
                return View(newCustomer);
            }

            newCustomer.Customer_id = Guid.NewGuid().ToString();
            ModelState.Remove("Customer_id");
            ModelState.Remove("driver_license_number");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.MsCustomers.Add(newCustomer);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Gagal menyimpan: " + ex.Message;
                }
            }
            else
            {
                var errorMsg = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                ViewBag.Error = "Data tidak lengkap: " + errorMsg;
            }

            return View(newCustomer);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "/")
        {
            var user = _context.MsCustomers
                               .FirstOrDefault(u => u.name == username && u.password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Customer_id),
                    new Claim(ClaimTypes.Name, user.name),
                    new Claim(ClaimTypes.Email, user.email ?? ""),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl != "/")
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Car");
            }

            ViewBag.Error = "Username atau Password salah!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
