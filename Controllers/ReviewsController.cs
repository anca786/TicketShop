using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

public class ReviewsController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 1. ADĂUGARE (POST)
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adauga([Bind("EvenimentId,Continut,Rating")] Review review)
    {
        // 1. Setăm manual Userul și Data
        review.UtilizatorId = _userManager.GetUserId(User);
        review.DataPostarii = DateTime.Now;

        // 2. IMPORTANT: Eliminăm erorile de validare pentru câmpurile pe care le completăm noi automat
        // Dacă nu facem asta, ModelState.IsValid va fi FALSE pentru că UtilizatorId era null la trimiterea form-ului
        ModelState.Remove(nameof(Review.Utilizator));
        ModelState.Remove(nameof(Review.UtilizatorId)); // <--- ASTA LIPSEA
        ModelState.Remove(nameof(Review.Eveniment));

        // 3. Verificăm acum validarea
        if (ModelState.IsValid)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Review-ul a fost adăugat!";
        }
        else
        {
            // Debugging: Poți vedea exact ce eroare apare dacă mai ai probleme
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["MesajEroare"] = "Eroare: " + string.Join(" | ", errors);
            // Mesajul vechi era: "Nu am putut adăuga review-ul..."
        }

        return RedirectToAction("Details", "Evenimente", new { id = review.EvenimentId });
    }

    // 2. ȘTERGERE (POST)
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sterge(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        var userId = _userManager.GetUserId(User);

        // Verificăm dacă ești proprietarul sau Admin
        if (review.UtilizatorId == userId || User.IsInRole("Admin"))
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Review șters cu succes.";
        }
        else
        {
            TempData["MesajEroare"] = "Nu ai dreptul să ștergi acest review.";
        }

        return RedirectToAction("Details", "Evenimente", new { id = review.EvenimentId });
    }

    // 3. EDITARE (GET) - Afișează pagina de editare
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _context.Reviews.Include(r => r.Eveniment).FirstOrDefaultAsync(r => r.Id == id);

        if (review == null) return NotFound();

        // Verificare proprietar
        if (review.UtilizatorId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        return View(review);
    }

    // 4. EDITARE (POST) - Salvează modificările
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Review reviewActualizat)
    {
        if (id != reviewActualizat.Id) return NotFound();

        var reviewOriginal = await _context.Reviews.FindAsync(id);
        if (reviewOriginal == null) return NotFound();

        // Verificare proprietar
        if (reviewOriginal.UtilizatorId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        // Actualizăm doar conținutul și ratingul
        reviewOriginal.Continut = reviewActualizat.Continut;
        reviewOriginal.Rating = reviewActualizat.Rating;
        reviewOriginal.DataPostarii = DateTime.Now; // Opțional: actualizăm data

        if (reviewActualizat.Continut.Length >= 10 && reviewActualizat.Continut.Length <= 500)
        {
            _context.Update(reviewOriginal);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Review modificat cu succes!";
            return RedirectToAction("Details", "Evenimente", new { id = reviewOriginal.EvenimentId });
        }

        return View(reviewActualizat);
    }
}