using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Models; // Asigură-te de namespace
using TicketShop.Models.ViewModels; // <--- Asta trebuie să apară

[Authorize]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Profile()
    {
        var userId = _userManager.GetUserId(User);

        // Încărcăm userul cu tot cu bilete și review-uri
        var user = await _userManager.Users
            .Include(u => u.Bilete)
                .ThenInclude(b => b.Eveniment) // Ca să vedem la ce eveniment e biletul
            .Include(u => u.Reviews)
                .ThenInclude(r => r.Eveniment) // Ca să vedem unde a lăsat review
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        return View(user); // Trimitem userul către View-ul Profile.cshtml
    }

    // 1. GET: Afișează formularul de setări
    public async Task<IActionResult> Settings()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var model = new UserSettingsViewModel
        {
            Nume = user.Nume,
            Prenume = user.Prenume,
            Email = user.Email,
        };

        return View(model);
    }

    // 2. POST: Salvează modificările
    [HttpPost]
    public async Task<IActionResult> Settings(UserSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        // A. Actualizăm datele de bază
        user.Nume = model.Nume;
        user.Prenume = model.Prenume;
        user.PhoneNumber = model.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            TempData["Eroare"] = "Nu s-au putut actualiza datele.";
            return View(model);
        }

        // B. Verificăm dacă vrea să schimbe parola
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (string.IsNullOrEmpty(model.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Trebuie să introduci parola actuală pentru a o schimba pe cea nouă.");
                return View(model);
            }

            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!passwordChangeResult.Succeeded)
            {
                foreach (var error in passwordChangeResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        TempData["Succes"] = "Profilul a fost actualizat cu succes!";
        return RedirectToAction("Profile");
    }
}