using System.ComponentModel.DataAnnotations.Schema;

namespace TicketShop.Models
{
    public class Bilet
    {
        public int Id { get; set; }
        public decimal Pret { get; set; }
        public bool Vandut { get; set; }

        public int EvenimentId { get; set; }
        public Eveniment Eveniment { get; set; }

        public int? CosId { get; set; } 

        [ForeignKey("CosId")]
        public virtual Cos? Cos { get; set; }

        public string? UserId { get; set; } 
        public virtual ApplicationUser? User { get; set; }
    }
}