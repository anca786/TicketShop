using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketShop.Models;

namespace TicketShop.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        // Asigură-te că ai liniile astea
        public DbSet<Eveniment> Evenimente { get; set; }
        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Bilet> Bilete { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Cos> Cosuri { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Wishlist (neschimbat)
            builder.Entity<Wishlist>()
                .HasMany(w => w.Evenimente)
                .WithMany();

            // --- CONFIGURAREA CORECTĂ PENTRU COȘ ---
            builder.Entity<Cos>()
                .HasMany(c => c.Bilete)       // Un coș -> multe bilete
                .WithOne(b => b.Cos)          // Un bilet -> un singur coș
                .HasForeignKey(b => b.CosId)
                .OnDelete(DeleteBehavior.SetNull); // Dacă ștergem coșul, nu ștergem biletul din DB

            // Preț
            builder.Entity<Bilet>()
                .Property(b => b.Pret)
                .HasColumnType("decimal(18,2)");
            builder.Entity<FAQ>().HasData(
        new FAQ
        {
            Id = 1, // La seeding TREBUIE să pui ID-ul manual
            Intrebare = "retur",
            Raspuns = "Poți returna biletele cu maxim 24 de ore înainte de eveniment. Banii intră în cont în 3 zile."
        },
        new FAQ
        {
            Id = 2,
            Intrebare = "contact",
            Raspuns = "Ne poți contacta la support@ticketshop.ro sau la telefon 0770.123.456."
        },
        new FAQ
        {
            Id = 3,
            Intrebare = "buna",
            Raspuns = "Bună! Sunt asistentul tău roz. Întreabă-mă despre bilete, cont sau evenimente! 💕"
        },
        new FAQ
        {
            Id = 4,
            Intrebare = "cont",
            Raspuns = "Poți crea un cont gratuit apăsând pe butonul Register din dreapta sus."
        },
        new FAQ
        {
            Id = 5,
            Intrebare = "locatie",
            Raspuns = "Locația evenimentului este scrisă pe biletul electronic pe care îl primești pe email."
        }
    );
        

    }
    }
}