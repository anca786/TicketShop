using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

public class EvenimenteController : Controller
{
    private readonly ApplicationDBContext _context;
    public EvenimenteController(ApplicationDBContext context)
    {
        _context = context;
    }

    // lista publică o lași cum este:
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

    // ADMIN: lista de management
    public async Task<IActionResult> AdminIndex()
    {
        var evenimente = await _context.Evenimente
                                       .Include(e => e.Categorie)
                                       .OrderBy(e => e.Data)
                                       .ToListAsync();
        return View(evenimente);
    }

    // GET: Evenimente/New
    public async Task<IActionResult> New()
    {
        ViewBag.Categorii = await _context.Categorii
                                          .OrderBy(c => c.Nume)
                                          .ToListAsync();
        return View();
    }

    // POST: Evenimente/New
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(Eveniment eveniment)
    {
        // Ignoră colecțiile la validare
        ModelState.Remove(nameof(Eveniment.Bilete));
        ModelState.Remove(nameof(Eveniment.Reviews));
        ModelState.Remove(nameof(Eveniment.Categorie));

        if (!ModelState.IsValid)
        {
            ViewBag.Categorii = await _context.Categorii.OrderBy(c => c.Nume).ToListAsync();
            return View(eveniment);
        }

        _context.Evenimente.Add(eveniment);
        await _context.SaveChangesAsync();
        TempData["message"] = "Evenimentul a fost adăugat cu succes!";
        return RedirectToAction(nameof(AdminIndex));
    }
    // GET: Evenimente/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var eveniment = await _context.Evenimente.FindAsync(id);
        if (eveniment == null)
            return NotFound();

        // dropdown categorii
        ViewBag.Categorii = await _context.Categorii
                                          .OrderBy(c => c.Nume)
                                          .ToListAsync();

        return View(eveniment);   // Views/Evenimente/Edit.cshtml
    }

    // POST: Evenimente/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Eveniment eveniment)
    {
        if (id != eveniment.Id)
            return NotFound();

        // Ignorăm colecțiile la validare
        ModelState.Remove(nameof(Eveniment.Bilete));
        ModelState.Remove(nameof(Eveniment.Reviews));
        ModelState.Remove(nameof(Eveniment.Categorie));

        if (!ModelState.IsValid)
        {
            ViewBag.Categorii = await _context.Categorii
                                              .OrderBy(c => c.Nume)
                                              .ToListAsync();
            return View(eveniment);
        }

        try
        {
            _context.Update(eveniment);
            await _context.SaveChangesAsync();
            TempData["message"] = "Evenimentul a fost modificat cu succes!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EvenimentExists(eveniment.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(AdminIndex));
    }

    // POST: Evenimente/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var eveniment = await _context.Evenimente.FindAsync(id);
        if (eveniment == null)
            return NotFound();

        _context.Evenimente.Remove(eveniment);   // șterge DOAR evenimentul
        await _context.SaveChangesAsync();
        TempData["message"] = "Evenimentul a fost șters cu succes!";
        return RedirectToAction(nameof(AdminIndex));
    }

    private bool EvenimentExists(int id)
    {
        return _context.Evenimente.Any(e => e.Id == id);
    }

}
