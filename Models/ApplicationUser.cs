using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // Necesar pentru List

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

        // --- RELAȚII CORECTATE ---

        // Inițializăm listele ca să nu primim "NullReferenceException" când userul nu are bilete
        public virtual ICollection<Bilet> Bilete { get; set; } = new List<Bilet>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Adăugăm '?' pentru a le face NULLABLE (Opționale)
        // Un user nou creat nu are încă wishlist sau coș
        public virtual Wishlist? Wishlist { get; set; }
        public virtual Cos? Cos { get; set; }
    }
}