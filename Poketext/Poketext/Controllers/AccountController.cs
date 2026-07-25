using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poketext.Models;

namespace Poketext.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User loginUser)
        {
            if (string.IsNullOrWhiteSpace(loginUser.Username) ||
                string.IsNullOrWhiteSpace(loginUser.Password))
            {
                ViewBag.Error = "Please complete all fields.";
                return View(loginUser);
            }

            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(loginUser);
            }

            PasswordVerificationResult result =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    loginUser.Password
                );

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(loginUser);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Error = "Please complete all fields.";
                return View(user);
            }

            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username == user.Username);

            if (usernameExists)
            {
                ViewBag.Error = "Username already exists.";
                return View(user);
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            user.CapturedPokemon = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Account created successfully. You can now log in.";

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}