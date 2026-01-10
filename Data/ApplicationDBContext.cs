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
        
        

    }
    }
}