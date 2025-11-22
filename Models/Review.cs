namespace TicketShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Continut { get; set; }
        public int Rating { get; set; } // 1-5

        public int EvenimentId { get; set; }
        public Eveniment Eveniment { get; set; }

        public string UtilizatorId { get; set; }
        public ApplicationUser Utilizator { get; set; }
    }
}
