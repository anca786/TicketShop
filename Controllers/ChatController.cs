using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

namespace TicketShop.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ChatController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetRaspuns([FromBody] MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new { response = "Nu ai scris nimic..." });
            }

            var mesajUser = request.Message.Trim().ToLower();

            // LOGICA DE CĂUTARE:
            // Căutăm o întrebare din baza de date care să fie conținută în ce a scris userul
            // SAU invers (dacă userul a scris doar un cuvânt cheie).
            var faq = await _context.FAQs
                .Where(f => mesajUser.Contains(f.Intrebare.ToLower()) || f.Intrebare.ToLower().Contains(mesajUser))
                .FirstOrDefaultAsync();

            string raspunsBot;

            if (faq != null)
            {
                raspunsBot = faq.Raspuns;
            }
            else
            {
                // Răspuns standard când nu știe
                raspunsBot = "Îmi pare rău, nu am înțeles întrebarea. Poți întreba despre: bilete, retur, locație, cont sau contact.";
            }

            return Json(new { response = raspunsBot });
        }
    }

    // Clasă pentru a primi datele din JavaScript
    public class MessageRequest
    {
        public string Message { get; set; }
    }
}