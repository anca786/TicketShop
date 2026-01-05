namespace TicketShop.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Intrebare { get; set; } // Ex: "Cum pot plăti?"
        public string Raspuns { get; set; }   // Ex: "Poți plăti cu cardul online."
    }
}