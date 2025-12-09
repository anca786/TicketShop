using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        [Required]
        public string UtilizatorId { get; set; }
        public ApplicationUser Utilizator { get; set; }

        // Relație Many-to-Many cu Eveniment
        public ICollection<Eveniment> Evenimente { get; set; }
    }
}
