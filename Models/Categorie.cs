namespace TicketShop.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nume { get; set; }

        //avem 1:N cu Eveniment
        public ICollection<Eveniment> Evenimente { get; set; }
    }
}
