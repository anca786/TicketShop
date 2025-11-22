using Microsoft.EntityFrameworkCore;

namespace TicketShop.Models
{
    



    public class Bilet
    {
        public int Id { get; set; }
        public decimal Pret { get; set; }
        public bool Vandut { get; set; }

        // Relație cu Eveniment
        public int EvenimentId { get; set; }
        public Eveniment Eveniment { get; set; }

        // Relație opțională cu Utilizator
        public string? UtilizatorId { get; set; }
        public ApplicationUser? Utilizator { get; set; }
    }
}
