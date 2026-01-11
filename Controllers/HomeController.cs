using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketShop.Models;
using TicketShop.Data;
using Microsoft.AspNetCore.Identity;

namespace TicketShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string cautare)
{
    var query = _context.Evenimente
        .Include(e => e.Categorie)
        .Where(e => e.Status == EventStatus.Approved) 
        .AsQueryable();

    if (!string.IsNullOrEmpty(cautare))
    {
        query = query.Where(e => e.Nume.Contains(cautare) || 
                                 e.Descriere.Contains(cautare) || 
                                 e.Locatie.Contains(cautare));
    }

    query = query.Where(e => e.Data >= DateTime.Now);
    query = query.OrderBy(e => e.Data);

    ViewData["CautareCurenta"] = cautare;

    if (string.IsNullOrEmpty(cautare)) 
    {
        query = query.Take(3);
    }

    return View(await query.ToListAsync());
}

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var eveniment = await _context.Evenimente
            .Include(e => e.Categorie)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (eveniment == null) return NotFound();

        int bileteDisponibile = await _context.Bilete
            .CountAsync(b => b.EvenimentId == id && !b.Vandut && b.CosId == null);

        ViewBag.Stoc = bileteDisponibile;


        bool isWishlisted = false;

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var wishlist = await _context.Wishlists
                    .Include(w => w.Evenimente)
                    .FirstOrDefaultAsync(w => w.UtilizatorId == user.Id);

                if (wishlist != null && wishlist.Evenimente.Any(e => e.Id == id))
                {
                    isWishlisted = true;
                }
            }
        }

        ViewBag.IsWishlisted = isWishlisted;

        return RedirectToAction("Details", "Evenimente", new { id = id });
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

    public IActionResult Despre()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult FAQ()
    {
        return View();
    }
}