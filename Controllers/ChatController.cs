using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;

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
        var mesajUser = request.Message.ToLower();

        // 1. Căutăm în baza de date un răspuns relevant
        // Căutăm dacă întrebarea din baza de date conține vreun cuvânt din ce a scris userul
        // Sau invers, dacă ce a scris userul conține cuvinte cheie din întrebarea stocată
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
            raspunsBot = "Îmi pare rău, încă învăț și nu am înțeles întrebarea. Te rog încearcă să reformulezi sau contactează echipa umană! 💖";
        }

        return Json(new { response = raspunsBot });
    }
}

// Clasă ajutătoare pentru a primi JSON-ul din JavaScript
public class MessageRequest
{
    public string Message { get; set; }
}