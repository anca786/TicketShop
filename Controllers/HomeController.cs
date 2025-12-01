using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketShop.Models;
using TicketShop.Data;

namespace TicketShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDBContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var evenimente = await _context.Evenimente
                                           .Include(e => e.Categorie)
                                           .OrderBy(e => e.Data)
                                           .Take(6)
                                           .ToListAsync();
        return View(evenimente);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eveniment = await _context.Evenimente
            .Include(e => e.Categorie)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (eveniment == null)
        {
            return NotFound();
        }

        return View(eveniment);
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
