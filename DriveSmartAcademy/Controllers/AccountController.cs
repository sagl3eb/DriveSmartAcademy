using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DriveSmartAcademy.Data;
using DriveSmartAcademy.Models;
using DriveSmartAcademy.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DriveSmartAcademy.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && VerifyPassword(model.Password, user.PasswordHash))
                {
                    // Create claims for the user
                    var claims = new List<Claim>
                   {
                       new Claim(ClaimTypes.Name, user.Email),
                       new Claim(ClaimTypes.GivenName, user.FirstName),
                       new Claim(ClaimTypes.Surname, user.LastName),
                       new Claim(ClaimTypes.Role, user.UserType),
                       new Claim("UserId", user.UserID.ToString())
                   };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Update LastLogin
                    user.LastLogin = DateTime.Now;
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Redirect based on user type
                    return user.UserType?.ToLower() switch
                    {
                        "admin" => RedirectToAction("Dashboard", "Admin"),
                        "instructor" => RedirectToAction("Dashboard", "Instructor"),
                        "student" => RedirectToAction("Dashboard", "Student"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                var user = new User
                {
                    UserID = Guid.NewGuid(),
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    StreetAddress = model.StreetAddress,
                    Region = model.Region,
                    City = model.City,
                    State = model.State,
                    UserType = "student",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Automatically log in user after registration
                var claims = new List<Claim>
               {
                   new Claim(ClaimTypes.Name, user.Email),
                   new Claim(ClaimTypes.GivenName, user.FirstName),
                   new Claim(ClaimTypes.Surname, user.LastName),
                   new Claim(ClaimTypes.Role, user.UserType),
                   new Claim("UserId", user.UserID.ToString())
               };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redirect based on user type
                return user.UserType?.ToLower() switch
                {
                    "admin" => RedirectToAction("Dashboard", "Admin"),
                    "instructor" => RedirectToAction("Dashboard", "Instructor"),
                    "student" => RedirectToAction("Dashboard", "Student"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // password hashing
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // password verification
        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
