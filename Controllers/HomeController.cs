using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketShop.Models;
using TicketShop.Data;
using Microsoft.AspNetCore.Identity; // AICI: Necesar pentru User Manager

namespace TicketShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context;
    // AICI: Adãugãm UserManager pentru a ?ti cine e logat
    private readonly UserManager<ApplicationUser> _userManager;

    // AICI: Actualizãm constructorul sã primeascã ?i userManager
    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string cautare)
{
    // 1. Pornim cu toate evenimentele (dar nu le aducem încã din bazã)
    var query = _context.Evenimente
        .Include(e => e.Categorie)
        .Where(e => e.Status == EventStatus.Approved) 
        .AsQueryable();

    // 2. Aplicãm filtrul DOAR dacã utilizatorul a scris ceva
    if (!string.IsNullOrEmpty(cautare))
    {
        query = query.Where(e => e.Nume.Contains(cautare) || 
                                 e.Descriere.Contains(cautare) || 
                                 e.Locatie.Contains(cautare));
    }

    query = query.Where(e => e.Data >= DateTime.Now);
    // 3. Ordonãm rezultatele (cele mai noi primele)
    query = query.OrderBy(e => e.Data);

    // Salvãm ce a cãutat userul ca sã-i rãmânã scris în cãsu?ã dupã refresh
    ViewData["CautareCurenta"] = cautare;

    if (string.IsNullOrEmpty(cautare)) 
    {
        query = query.Take(3);
    }

    // 4. Executãm interogarea ?i trimitem lista
    return View(await query.ToListAsync());
}

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var eveniment = await _context.Evenimente
            .Include(e => e.Categorie)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (eveniment == null) return NotFound();

        // 1. COD VECHI: Numãrãm biletele disponibile (Stoc)
        int bileteDisponibile = await _context.Bilete
            .CountAsync(b => b.EvenimentId == id && !b.Vandut && b.CosId == null);

        ViewBag.Stoc = bileteDisponibile;


        // 2. COD NOU: Verificãm Wishlist-ul (Inimioara)
        bool isWishlisted = false;

        // Verificãm doar dacã userul este logat
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // Cãutãm în baza de date wishlist-ul userului
                var wishlist = await _context.Wishlists
                    .Include(w => w.Evenimente)
                    .FirstOrDefaultAsync(w => w.UtilizatorId == user.Id);

                // Dacã are wishlist ?i evenimentul e în el -> TRUE
                if (wishlist != null && wishlist.Evenimente.Any(e => e.Id == id))
                {
                    isWishlisted = true;
                }
            }
        }

        // Trimitem rezultatul (true/false) în View ca sã ?tim dacã colorãm inima
        ViewBag.IsWishlisted = isWishlisted;
        // ---------------------------------------------------------

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