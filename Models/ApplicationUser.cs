using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 30 caractere")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]*$", ErrorMessage = "Numele introdus conține caractere nepermise")]
        public string Nume { get; set; }
        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Prenumele trebuie să aibă între 3 și 50 caractere")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]*$", ErrorMessage = "Numele introdus conține caractere nepermise")]
        public string Prenume { get; set; }

        // Relații
        public ICollection<Bilet> Bilete { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Wishlist Wishlist { get; set; }
        public Cos Cos { get; set; }
    }
}
