using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

namespace TicketShop.Controllers
{
    public class CategoriiController : Controller
    {
        private readonly ApplicationDBContext _context;

        public async Task<IActionResult> AdminIndex()
        {
            var categorii = await _context.Categorii
                                          .Include(c => c.Evenimente)
                                          .ToListAsync();
            return View(categorii);   
        }

       

        public CategoriiController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorii = await _context.Categorii
                                          .Include(c => c.Evenimente)
                                          .ToListAsync();
            return View(categorii);
        }
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Categorie categorie)
        {
            //incerca sa valideze aiurea si evenimente 
            ModelState.Remove(nameof(Categorie.Evenimente));

            if (!ModelState.IsValid)
            {
                return View(categorie);
            }

            _context.Add(categorie);
            await _context.SaveChangesAsync();
            TempData["message"] = "Categoria a fost adăugată cu succes!";
            return RedirectToAction(nameof(AdminIndex));
        }
        // Edit - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categorii.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categorie categorie)
        {
            if (id != categorie.Id)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(Categorie.Evenimente));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Categoria a fost modificată cu succes!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(categorie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var categorie = await _context.Categorii.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }

            _context.Categorii.Remove(categorie);
            await _context.SaveChangesAsync();
            TempData["message"] = "Categoria a fost ștearsă cu succes!";

            return RedirectToAction(nameof(AdminIndex));
        }

        private bool CategorieExists(int id)
        {
            return _context.Categorii.Any(e => e.Id == id);
        }


    }
}