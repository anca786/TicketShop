using Microsoft.AspNetCore.Identity;

namespace TicketShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }

        // Relații
        public ICollection<Bilet> Bilete { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Wishlist Wishlist { get; set; }
        public Cos Cos { get; set; }
    }
}
