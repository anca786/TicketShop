using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

[Authorize] // Doar userii logați au coș
public class CosController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CosController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Afișează coșul
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var cos = await _context.Cosuri
            .Include(c => c.Bilete)
            .ThenInclude(b => b.Eveniment)
            .FirstOrDefaultAsync(c => c.UtilizatorId == user.Id);

        if (cos == null)
        {
            cos = new Cos { UtilizatorId = user.Id, Bilete = new List<Bilet>() };
        }

        return View(cos);
    }

    // Adaugă în coș
    // Modifică parametrul din 'biletId' în 'evenimentId'
    // Verifică parametrul: trebuie să fie 'int evenimentId'
    public async Task<IActionResult> Adauga(int evenimentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // 1. Verificăm stocul (căutăm un bilet liber)
        var biletDisponibil = await _context.Bilete
            .FirstOrDefaultAsync(b => b.EvenimentId == evenimentId
                                   && !b.Vandut
                                   && b.CosId == null);

        // 2. PROTECȚIE: Dacă nu e stoc, trimitem înapoi la HOME cu eroare
        if (biletDisponibil == null)
        {
            TempData["MesajEroare"] = "Nu mai sunt bilete disponibile!";
            // AICI ERA PROBLEMA: Trebuie Controller="Home", nu "Evenimente"
            return RedirectToAction("Details", "Home", new { id = evenimentId });
        }

        // 3. Gestionăm coșul
        var cos = await _context.Cosuri
            .Include(c => c.Bilete)
            .FirstOrDefaultAsync(c => c.UtilizatorId == user.Id);

        if (cos == null)
        {
            cos = new Cos { UtilizatorId = user.Id };
            _context.Cosuri.Add(cos);
            await _context.SaveChangesAsync();
        }

        // 4. Adăugăm biletul
        if (!cos.Bilete.Any(b => b.Id == biletDisponibil.Id))
        {
            biletDisponibil.CosId = cos.Id;
            _context.Update(biletDisponibil);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Bilet adăugat în coș!";
        }

        // 5. Redirecționăm corect la HOME
        return RedirectToAction("Details", "Home", new { id = evenimentId });
    }
    // Șterge din coș
    public async Task<IActionResult> Sterge(int biletId)
    {
        var user = await _userManager.GetUserAsync(User);
        var cos = await _context.Cosuri.Include(c => c.Bilete).FirstOrDefaultAsync(c => c.UtilizatorId == user.Id);

        if (cos != null)
        {
            var bilet = cos.Bilete.FirstOrDefault(b => b.Id == biletId);
            if (bilet != null)
            {
                cos.Bilete.Remove(bilet);
                await _context.SaveChangesAsync();
            }
        }
        return RedirectToAction("Index");
    }
}