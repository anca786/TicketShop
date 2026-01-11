using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TicketShop.Data;
using TicketShop.Models;

namespace TicketShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var wishlist = await _context.Wishlists
                .Include(w => w.Evenimente)
                .ThenInclude(e => e.Categorie)
                .FirstOrDefaultAsync(w => w.UtilizatorId == user.Id);

            if (wishlist == null)
            {
                return View(new List<Eveniment>());
            }

            return View(wishlist.Evenimente);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int evenimentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Trebuie să fii logat!" });
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.Evenimente)
                .FirstOrDefaultAsync(w => w.UtilizatorId == user.Id);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UtilizatorId = user.Id,
                    Evenimente = new List<Eveniment>()
                };
                _context.Wishlists.Add(wishlist);
            }

            if (wishlist.Evenimente == null)
            {
                wishlist.Evenimente = new List<Eveniment>();
            }

            var eveniment = await _context.Evenimente.FindAsync(evenimentId);
            if (eveniment == null) return Json(new { success = false, message = "Eveniment inexistent" });

            bool isAdded = false;

            if (wishlist.Evenimente.Any(e => e.Id == evenimentId))
            {
                wishlist.Evenimente.Remove(eveniment);
                isAdded = false;
            }
            else
            {
                wishlist.Evenimente.Add(eveniment);
                isAdded = true;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, isAdded = isAdded });
        }

        public async Task<IActionResult> Sterge(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var wishlist = await _context.Wishlists
                .Include(w => w.Evenimente)
                .FirstOrDefaultAsync(w => w.UtilizatorId == user.Id);

            if (wishlist != null)
            {
                var eveniment = wishlist.Evenimente.FirstOrDefault(e => e.Id == id);
                if (eveniment != null)
                {
                    wishlist.Evenimente.Remove(eveniment);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}