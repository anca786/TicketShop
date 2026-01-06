using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketShop.Models
{
    // stari posibile ale evenimentului
        public enum EventStatus
        {
            Pending,   
            Approved,  
            Rejected  
        }
    public class Eveniment
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele evenimentului este obligatoriu.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 200 de caractere.")] 
        public string Nume { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        [StringLength(2000, ErrorMessage = "Descrierea nu poate depăși 2000 de caractere.")]
        [DataType(DataType.MultilineText)]
        public string Descriere { get; set; }

        [Required(ErrorMessage = "Data evenimentului este obligatorie.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data și Ora")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Locația este obligatorie.")]
        [StringLength(100, ErrorMessage = "Locația nu poate depăși 100 de caractere.")]
        public string Locatie { get; set; }

        [Display(Name = "Link Imagine (URL)")]
        public string? ImagineUrl { get; set; }

        [NotMapped]
        [Display(Name = "Încarcă Imagine")]
        public IFormFile? ImagineFisier { get; set; }

        // Relație cu categorie
        [Display(Name = "Categorie")]
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        // Relație 1:N cu Bilet și Review
        public ICollection<Bilet> Bilete { get; set; }
        public ICollection<Review> Reviews { get; set; }
        [Column(TypeName = "decimal(18,2)")] // Formatare pentru bani
        public decimal Pret { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Pending; 

        public string? MotivRespingere { get; set; } 

        public string? OrganizatorId { get; set; }
    }
}
