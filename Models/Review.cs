using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Te rog scrie o părere despre eveniment.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Comentariul trebuie să aibă între 10 și 500 de caractere.")]
        [DataType(DataType.MultilineText)]
        public string Continut { get; set; }

        [Required(ErrorMessage = "Te rog selectează o notă.")]
        [Range(1, 5, ErrorMessage = "Nota trebuie să fie între 1 și 5.")]
        public int Rating { get; set; }

        [Required]
        public int EvenimentId { get; set; }
        public Eveniment Eveniment { get; set; }

        [Required]
        public string UtilizatorId { get; set; }
        public ApplicationUser Utilizator { get; set; }
        public DateTime DataPostarii { get; set; } = DateTime.Now;
    }
}
