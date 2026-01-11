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

            var faq = await _context.FAQs
                .Where(f => mesajUser.Contains(f.Intrebare.ToLower()) || f.Intrebare.ToLower().Contains(mesajUser))
                .FirstOrDefaultAsync();

            if (faq != null)
            {
                return Json(new { response = faq.Raspuns });
            }

            var evenimentGasit = await _context.Evenimente
                .Where(e => e.Descriere.ToLower().Contains(mesajUser) || e.Nume.ToLower().Contains(mesajUser))
                .FirstOrDefaultAsync();

            if (evenimentGasit != null)
            {
                return Json(new { response = $"Am găsit ceva legat de asta la evenimentul '{evenimentGasit.Nume}': {evenimentGasit.Descriere.Substring(0, Math.Min(100, evenimentGasit.Descriere.Length))}..." });
            }

            return Json(new { response = "Nu am găsit informații, dar poți contacta un administrator." });
        }
    }

    public class MessageRequest
    {
        public string Message { get; set; }
    }
}