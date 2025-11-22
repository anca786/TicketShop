namespace TicketShop.Models
{
    public class Cos
    {
        public int Id { get; set; }

        public string UtilizatorId { get; set; }
        public ApplicationUser Utilizator { get; set; }

        // Relație Many-to-Many cu Bilet
        public ICollection<Bilet> Bilete { get; set; }
    }
}
