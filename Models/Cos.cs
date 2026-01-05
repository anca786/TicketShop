using System.Collections.Generic;

namespace TicketShop.Models
{
    public class Cos
    {
        public int Id { get; set; }
        public string UtilizatorId { get; set; }

        public virtual ICollection<Bilet> Bilete { get; set; } = new List<Bilet>();
    }
}