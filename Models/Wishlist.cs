using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        [Required]
        public string UtilizatorId { get; set; }
        public ApplicationUser Utilizator { get; set; }

        public ICollection<Eveniment> Evenimente { get; set; }
    }
}
