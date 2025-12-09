using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketShop.Models
{
    



    public class Bilet
    {
        public int Id { get; set; }

        [Range(0, 100000, ErrorMessage = "Prețul trebuie să fie o valoare pozitivă.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
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
