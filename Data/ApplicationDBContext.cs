using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketShop.Models;


namespace TicketShop.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurare many-to-many pentru Wishlist
            builder.Entity<Wishlist>()
                .HasMany(w => w.Evenimente)
                .WithMany();

            // Configurare many-to-many pentru Cos
            builder.Entity<Cos>()
                .HasMany(c => c.Bilete)
                .WithMany();
            
            builder.Entity<Bilet>()
                .Property(b => b.Pret)
                .HasColumnType("decimal(18,2)");
        }

    }
}
