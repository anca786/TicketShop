using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static TicketShop.Models.Eveniment;
using Microsoft.Data.SqlClient;

public class EvenimenteController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EvenimenteController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string searchString, int? categoryId, string sortOrder)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
        ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
        ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";
        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentCategory"] = categoryId;

        var evenimente = _context.Evenimente
            .Include(e => e.Categorie)
            .Where(e => e.Status == EventStatus.Approved)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            evenimente = evenimente.Where(e => e.Nume.Contains(searchString));
        }

        if (categoryId.HasValue && categoryId != 0)
        {
            evenimente = evenimente.Where(e => e.CategorieId == categoryId);
        }

        switch (sortOrder)
        {
            case "date_desc":
                evenimente = evenimente.OrderByDescending(e => e.Data);
                break;
            case "Price":
                evenimente = evenimente.OrderBy(e => e.Pret); // Preț mic -> mare
                break;
            case "price_desc":
                evenimente = evenimente.OrderByDescending(e => e.Pret); // Preț mare -> mic
                break;
            case "Rating":
                evenimente = evenimente.OrderBy(e => e.RatingMediu); // Rating mic -> mare
                break;
            case "rating_desc":
                evenimente = evenimente.OrderByDescending(e => e.RatingMediu); // Cel mai popular (Rating mare -> mic)
                break;
            default:
                // Implicit: Data cea mai apropiată (Ascendent)
                evenimente = evenimente.OrderBy(e => e.Data);
                break;
        }


        ViewBag.Categorii = new SelectList(await _context.Categorii.ToListAsync(), "Id", "Nume", categoryId);


        return View(await evenimente.ToListAsync());
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var eveniment = await _context.Evenimente
            .AsNoTracking()
            .Include(e => e.Categorie)
            .Include(e => e.Bilete)
            .Include(e => e.Reviews.OrderByDescending(r => r.DataPostarii))
                .ThenInclude(r => r.Utilizator)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (eveniment == null) return NotFound();
        int bileteDisponibile = eveniment.Bilete.Count(b => b.Vandut == false && b.CosId == null);

        ViewBag.Stoc = bileteDisponibile;

        return View("~/Views/Home/Details.cshtml", eveniment);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminIndex()
    {
        var evenimente = await _context.Evenimente
                                       .Include(e => e.Categorie)
                                       .OrderByDescending(e => e.Data)
                                       .ToListAsync();
        return View(evenimente);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        var ev = await _context.Evenimente.FindAsync(id);
        if (ev != null)
        {
            ev.Status = EventStatus.Approved;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(AdminIndex));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(int id)
    {
        var ev = await _context.Evenimente.FindAsync(id);
        if (ev != null)
        {
            ev.Status = EventStatus.Rejected;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(AdminIndex));
    }

    [Authorize(Roles = "Admin,Colaborator")]
    public async Task<IActionResult> New()
    {
        ViewBag.Categorii = await _context.Categorii.OrderBy(c => c.Nume).ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Colaborator")]
    public async Task<IActionResult> New(Eveniment eveniment)
    {
        if (User.IsInRole("Admin"))
        {
            eveniment.Status = EventStatus.Approved; // Sau EventStatus.Approved, depinde cum ai tu enum-ul
        }
        else
        {
            eveniment.Status = EventStatus.Pending;
        }

        eveniment.OrganizatorId = _userManager.GetUserId(User);

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
            ViewBag.Categorii = await _context.Categorii.OrderBy(c => c.Nume).ToListAsync();
            return View(eveniment);
        }

        _context.Evenimente.Add(eveniment);
        await _context.SaveChangesAsync();

        if (User.IsInRole("Admin"))
        {
            TempData["message"] = "Evenimentul a fost creat și publicat cu succes!";
            return RedirectToAction(nameof(AdminIndex));
        }
        else
        {
            TempData["message"] = "Evenimentul a fost trimis spre aprobare!";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var eveniment = await _context.Evenimente.FindAsync(id);
        if (eveniment == null) return NotFound();
        ViewBag.Categorii = await _context.Categorii.OrderBy(c => c.Nume).ToListAsync();
        return View(eveniment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Eveniment eveniment)
    {
        if (id != eveniment.Id) return NotFound();

        var oldEvent = await _context.Evenimente.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (oldEvent != null)
        {
            eveniment.Status = oldEvent.Status;
            eveniment.OrganizatorId = oldEvent.OrganizatorId;
            if (eveniment.ImagineFisier == null) eveniment.ImagineUrl = oldEvent.ImagineUrl;
        }

        ModelState.Remove(nameof(Eveniment.Bilete));
        ModelState.Remove(nameof(Eveniment.Reviews));
        ModelState.Remove(nameof(Eveniment.Categorie));

        if (eveniment.ImagineFisier != null)
        {
            var extension = Path.GetExtension(eveniment.ImagineFisier.FileName).ToLower();
            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await eveniment.ImagineFisier.CopyToAsync(stream);
            }
            eveniment.ImagineUrl = "/images/" + fileName;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categorii = await _context.Categorii.OrderBy(c => c.Nume).ToListAsync();
            return View(eveniment);
        }

        _context.Update(eveniment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(AdminIndex));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var eveniment = await _context.Evenimente.FindAsync(id);
        if (eveniment != null)
        {
            _context.Evenimente.Remove(eveniment);
            await _context.SaveChangesAsync();
        }
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
            eveniment.Pret = pret;
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