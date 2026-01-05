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

    // GET: Evenimente
    // GET: Evenimente
    public async Task<IActionResult> Index(string searchString, int? categoryId) // Păstrează parametrul categoryId mic
    {
        // 1. Includem Categoria pentru afișare
        var evenimente = _context.Evenimente.Include(e => e.Categorie).AsQueryable();

        // 2. Filtrare după Text
        if (!string.IsNullOrEmpty(searchString))
        {
            evenimente = evenimente.Where(e => e.Nume.Contains(searchString));
        }

        // 3. Filtrare după Categorie
        // AICI ERA PROBLEMA: Probabil la tine în model proprietatea se numește 'CategorieId' sau verificăm după obiect
        if (categoryId.HasValue && categoryId != 0)
        {
            // Încercăm varianta cea mai sigură: verificăm ID-ul obiectului Categorie
            evenimente = evenimente.Where(e => e.Categorie.Id == categoryId);
        }

        // 4. Dropdown-ul
        // AICI ERA A DOUA PROBLEMĂ: La tine tabela se numește probabil 'Categorii' (nu Categories)
        // Verificăm dacă _context.Categorii există. Dacă nu, lasă-mi un comentariu.
        ViewBag.Categorii = new SelectList(await _context.Categorii.ToListAsync(), "Id", "Nume", categoryId);

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentCategory"] = categoryId;

        return View(await evenimente.ToListAsync());
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

        if (eveniment.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Data evenimentului trebuie să fie în viitor.");
        }

        if (eveniment.ImagineFisier != null && eveniment.ImagineFisier.Length > 0)
        {
            // 1. Validare Mărime (Max 5 MB)
            if (eveniment.ImagineFisier.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImagineFisier", "Imaginea este prea mare (maxim 5MB).");
            }

            // 2. Validare Format
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(eveniment.ImagineFisier.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImagineFisier", "Format neacceptat.");
            }

            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await eveniment.ImagineFisier.CopyToAsync(stream);
                }
                eveniment.ImagineUrl = "/images/" + fileName;
            }
        }

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

        if (eveniment.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Data evenimentului trebuie să fie în viitor.");
        }

        if (eveniment.ImagineFisier != null && eveniment.ImagineFisier.Length > 0)
        {
            if (eveniment.ImagineFisier.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImagineFisier", "Imaginea este prea mare (maxim 5MB).");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(eveniment.ImagineFisier.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImagineFisier", "Format neacceptat.");
            }

            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await eveniment.ImagineFisier.CopyToAsync(stream);
                }

                eveniment.ImagineUrl = "/images/" + fileName;
            }
        }

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
    // POST: Evenimente/GenereazaBilete
    [HttpPost]
    public async Task<IActionResult> GenereazaBilete(int id, int numarBilete, decimal pret)
    {
        var eveniment = await _context.Evenimente.FindAsync(id);
        if (eveniment == null) return NotFound();

        if (numarBilete > 0 && pret > 0)
        {
            var bileteNoi = new List<Bilet>();

            for (int i = 0; i < numarBilete; i++)
            {
                bileteNoi.Add(new Bilet
                {
                    EvenimentId = id,
                    Pret = pret,
                    Vandut = false,
                    CosId = null // E liber
                });
            }

            await _context.Bilete.AddRangeAsync(bileteNoi);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"Au fost generate {numarBilete} bilete cu succes!";
        }

        // Ne întoarcem la pagina de administrare (AdminIndex) sau Index
        return RedirectToAction(nameof(AdminIndex));
    }

    private bool EvenimentExists(int id)
    {
        return _context.Evenimente.Any(e => e.Id == id);
    }

}
