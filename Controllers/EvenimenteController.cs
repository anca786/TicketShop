using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;

namespace TicketShop.Controllers
{

    public class EvenimenteController : Controller
    {
        private readonly ApplicationDBContext _context;
        public EvenimenteController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var eventsQuery = _context.Evenimente.Include(e => e.Categorie).AsQueryable();
            if (categoryId.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.CategorieId == categoryId);
            }
            var evenimente = await eventsQuery.OrderBy(e => e.Data).ToListAsync();
            return View(evenimente);
        }
    }
}
