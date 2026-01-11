using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

[Authorize] 
public class CosController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CosController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

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

    public async Task<IActionResult> Adauga(int evenimentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();


        var biletDisponibil = await _context.Bilete
            .FirstOrDefaultAsync(b => b.EvenimentId == evenimentId
                                   && !b.Vandut
                                   && b.CosId == null);

        if (biletDisponibil == null)
        {
            TempData["MesajEroare"] = "Nu mai sunt bilete disponibile!";
            return RedirectToAction("Details", "Home", new { id = evenimentId });
        }

        var cos = await _context.Cosuri
            .Include(c => c.Bilete)
            .FirstOrDefaultAsync(c => c.UtilizatorId == user.Id);

        if (cos == null)
        {
            cos = new Cos { UtilizatorId = user.Id };
            _context.Cosuri.Add(cos);
            await _context.SaveChangesAsync();
        }

        if (!cos.Bilete.Any(b => b.Id == biletDisponibil.Id))
        {
            biletDisponibil.CosId = cos.Id;
            _context.Update(biletDisponibil);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Bilet adăugat în coș!";
        }

        return RedirectToAction("Details", "Home", new { id = evenimentId });
    }


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


    [Authorize]
    public async Task<IActionResult> FinalizeazaComanda()
    {
        var userId = _userManager.GetUserId(User);

        var cos = await _context.Cosuri
            .Include(c => c.Bilete)
            .FirstOrDefaultAsync(c => c.UtilizatorId == userId);

        if (cos != null && cos.Bilete.Any())
        {
            foreach (var bilet in cos.Bilete)
            {
                bilet.CosId = null;
                bilet.Vandut = true;
                bilet.UserId = userId;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comanda a fost finalizată cu succes! Mulțumim.";
        }
        else
        {
            TempData["ErrorMessage"] = "Coșul tău este gol.";
        }

        return RedirectToAction("Index", "Home");
    }
}