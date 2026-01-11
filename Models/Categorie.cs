using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Numele categoriei este obligatoriu.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între {2} și {1} caractere")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_]*$", ErrorMessage = "Numele introdus conține caractere nepermise")]
        public string Nume { get; set; }

        public ICollection<Eveniment> Evenimente { get; set; }
    }
}
