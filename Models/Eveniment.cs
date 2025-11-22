namespace TicketShop.Models
{
    public class Eveniment
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public DateTime Data { get; set; }
        public string Locatie { get; set; }

        // Relație cu categorie
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        // Relație 1:N cu Bilet și Review
        public ICollection<Bilet> Bilete { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
