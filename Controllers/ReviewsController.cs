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

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adauga([Bind("EvenimentId,Continut,Rating")] Review review)
    {
        review.UtilizatorId = _userManager.GetUserId(User);
        review.DataPostarii = DateTime.Now;

        ModelState.Remove(nameof(Review.Utilizator));
        ModelState.Remove(nameof(Review.UtilizatorId)); 
        ModelState.Remove(nameof(Review.Eveniment));

        if (ModelState.IsValid)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Review-ul a fost adăugat!";
            await ActualizeazaRatingEveniment(review.EvenimentId);
        }
        else
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["MesajEroare"] = "Eroare: " + string.Join(" | ", errors);
        }

        return RedirectToAction("Details", "Evenimente", new { id = review.EvenimentId });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sterge(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        var userId = _userManager.GetUserId(User);

        if (review.UtilizatorId == userId || User.IsInRole("Admin"))
        {
            int evenimentId = review.EvenimentId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            TempData["MesajSucces"] = "Review șters cu succes.";
            await ActualizeazaRatingEveniment(evenimentId);
        }
        else
        {
            TempData["MesajEroare"] = "Nu ai dreptul să ștergi acest review.";
        }

        return RedirectToAction("Details", "Evenimente", new { id = review.EvenimentId });
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var review = await _context.Reviews.Include(r => r.Eveniment).FirstOrDefaultAsync(r => r.Id == id);

        if (review == null) return NotFound();

        if (review.UtilizatorId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        return View(review);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Review reviewActualizat)
    {
        if (id != reviewActualizat.Id) return NotFound();

        var reviewOriginal = await _context.Reviews.FindAsync(id);
        if (reviewOriginal == null) return NotFound();

        if (reviewOriginal.UtilizatorId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        reviewOriginal.Continut = reviewActualizat.Continut;
        reviewOriginal.Rating = reviewActualizat.Rating;
        reviewOriginal.DataPostarii = DateTime.Now; 

        if (reviewActualizat.Continut.Length >= 10 && reviewActualizat.Continut.Length <= 500)
        {
            _context.Update(reviewOriginal);
            await _context.SaveChangesAsync();
            await ActualizeazaRatingEveniment(reviewOriginal.EvenimentId);
            TempData["MesajSucces"] = "Review modificat cu succes!";
            return RedirectToAction("Details", "Evenimente", new { id = reviewOriginal.EvenimentId });

        }

        return View(reviewActualizat);
    }

    private async Task ActualizeazaRatingEveniment(int evenimentId)
    {
        var eveniment = await _context.Evenimente
            .Include(e => e.Reviews)
            .FirstOrDefaultAsync(e => e.Id == evenimentId);

        if (eveniment != null)
        {
            if (eveniment.Reviews != null && eveniment.Reviews.Any())
            {
                double media = eveniment.Reviews.Average(r => r.Rating);

                eveniment.RatingMediu = Math.Round(media, 2);
            }
            else
            {
                eveniment.RatingMediu = 0;
            }

            _context.Update(eveniment);
            await _context.SaveChangesAsync();
        }
    }
}