using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketShop.Data;
using TicketShop.Models;
using System.Threading.Tasks;

public static class DbSeeder
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDBContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        string[] roleNames = { "Admin", "Colaborator", "Utilizator" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        if (!context.Categorii.Any())
        {
            var categorii = new Categorie[]
            {
                new Categorie { Nume = "Concert" },
                new Categorie { Nume = "Sport" },
                new Categorie { Nume = "Teatru" }
            };
            context.Categorii.AddRange(categorii);
            await context.SaveChangesAsync();
        }

        var concert = context.Categorii.First(c => c.Nume == "Concert");
        var sport = context.Categorii.First(c => c.Nume == "Sport");

        if (!context.Evenimente.Any())
        {
            var evenimente = new Eveniment[]
            {
                new Eveniment
                {
                    Nume = "Taylor Swift - The Eras Tour",
                    Descriere = "Cel mai mare spectacol al anului.",
                    Data = DateTime.Now.AddDays(30),
                    Locatie = "Arena Națională, București",
                    CategorieId = concert.Id,
                    ImagineUrl = "/imagini/imag_concerte.webp"
                },
                new Eveniment
                {
                    Nume = "Finala Campionatului de Tenis",
                    Descriere = "Cei mai buni jucători se întâlnesc la Paris.",
                    Data = DateTime.Now.AddMonths(2),
                    Locatie = "Roland Garros, Paris",
                    CategorieId = sport.Id,
                    ImagineUrl = "/imagini/imag_tenis.webp"
                },
                new Eveniment
                {
                    Nume = "Gala de Balet 'Lacul Lebedelor'",
                    Descriere = "Clasicul lui Ceaikovski, spectacol de neuitat.",
                    Data = DateTime.Now.AddDays(15),
                    Locatie = "Opera Națională, Cluj",
                    CategorieId = context.Categorii.First(c => c.Nume == "Teatru").Id,
                    ImagineUrl = "/imagini/imag_balet.webp"
                },
                new Eveniment
                {
                    Nume = "Festivalul Untold 2026",
                    Descriere = "Cea mai mare petrecere din Transilvania.",
                    Data = DateTime.Parse("2026-08-05"),
                    Locatie = "Cluj Arena, Cluj-Napoca",
                    CategorieId = concert.Id,
                    ImagineUrl = "/imagini/imag_festival.webp"
                },
                new Eveniment
                {
                    Nume = "Derby-ul de Fotbal al Capitalei",
                    Descriere = "Meci incendiar între rivalii din București.",
                    Data = DateTime.Now.AddDays(7),
                    Locatie = "Stadionul Steaua, București",
                    CategorieId = sport.Id,
                    ImagineUrl = "/imagini/imag_fotbal.webp"
                }
            };
            context.Evenimente.AddRange(evenimente);
            await context.SaveChangesAsync();
        }

        const string defaultPassword = "DemoUser123!";

        if (await userManager.FindByEmailAsync("admin@shop.ro") == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@shop.ro",
                Email = "admin@shop.ro",
                Nume = "Popescu",
                Prenume = "Andrei",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, defaultPassword);
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        if (await userManager.FindByEmailAsync("colaborator@shop.ro") == null)
        {
            var user1 = new ApplicationUser
            {
                UserName = "colaborator@shop.ro",
                Email = "colaborator@shop.ro",
                Nume = "Ionescu",
                Prenume = "Maria",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user1, defaultPassword);
            await userManager.AddToRoleAsync(user1, "Colaborator");
        }

        if (await userManager.FindByEmailAsync("client@shop.ro") == null)
        {
            var user2 = new ApplicationUser
            {
                UserName = "client@shop.ro",
                Email = "client@shop.ro",
                Nume = "Vasilescu",
                Prenume = "Cristian",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user2, defaultPassword);
            await userManager.AddToRoleAsync(user2, "Utilizator");
        }
    }
}