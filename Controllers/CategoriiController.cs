using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;

namespace TicketShop.Controllers
{
    public class CategoriiController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CategoriiController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorii = await _context.Categorii
                                          .Include(c => c.Evenimente)
                                          .ToListAsync();
            return View(categorii);
        }
    }
}