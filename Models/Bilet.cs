using System.ComponentModel.DataAnnotations.Schema;

namespace TicketShop.Models
{
    public class Bilet
    {
        public int Id { get; set; }
        public decimal Pret { get; set; }
        public bool Vandut { get; set; }

        // Restul proprietăților tale (Eveniment, Categorie etc.)
        public int EvenimentId { get; set; }
        public Eveniment Eveniment { get; set; }

        // --- PARTEA NOUĂ ---
        public int? CosId { get; set; } // Nullable!

        [ForeignKey("CosId")]
        public virtual Cos? Cos { get; set; }
    }
}