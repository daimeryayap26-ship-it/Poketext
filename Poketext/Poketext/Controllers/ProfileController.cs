using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poketext.Models;

namespace Poketext.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            var pokemons = await _context.Pokemons
                .OrderBy(p => p.Id)
                .ToListAsync();

            var capturedPokemonIds = await _context.UserPokemons
                .Where(up => up.UserId == userId)
                .Select(up => up.PokemonId)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                User = user,
                Pokemons = pokemons,
                CapturedPokemonIds = capturedPokemonIds
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCollection(
            List<int> capturedPokemonIds)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            capturedPokemonIds ??= new List<int>();

            var existingCaptured = await _context.UserPokemons
                .Where(up => up.UserId == userId)
                .ToListAsync();

            _context.UserPokemons.RemoveRange(existingCaptured);

            var validPokemonIds = await _context.Pokemons
                .Where(p => capturedPokemonIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            foreach (int pokemonId in validPokemonIds)
            {
                var userPokemon = new UserPokemon
                {
                    UserId = userId.Value,
                    PokemonId = pokemonId
                };

                _context.UserPokemons.Add(userPokemon);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.CapturedPokemon = validPokemonIds.Count;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Pokémon collection updated successfully.";

            return RedirectToAction("Index");
        }
    }
}