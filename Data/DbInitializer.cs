using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _420_15D_FX_H26_TP1.Data
{
    public static class DbInitializer
    {

        public static async Task Initialize(ApplicationDbContext context,
           UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            }
            if (!await userManager.Users.AnyAsync())
            {
                Utilisateur adminUser = new Utilisateur()
                {
                    UserName = "admin@cegepgarneau.ca",
                    NomUtilisateur = "snake",
                    Email = "admin@cegepgarneau.ca",
                    Courriel = "admin@cegepgarneau.ca",
                    Nom = "Admin",
                    Prenom = "yann",
                    Ville = "Sherbrooke",
                    CodePostal = "G1M2V8",
                    Adresse = "123 rue du marabout"

                };
                var resultAdmin = await userManager.CreateAsync(adminUser, "Password123!");
                if (resultAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");  // ✅ CORRECTION ICI
                }

                Utilisateur invite = new Utilisateur()
                {
                    UserName = "elyan@yahoo.com",
                    NomUtilisateur= "WOLF",
                    Email = "elyan@yahoo.com",
                    Courriel = "elyan@yahoo.com",
                    Nom = "ghost",
                    Prenom = "loic",
                    Ville = "Sherbrooke",
                    CodePostal = "G1H5V7",
                    Adresse = "123 rue du roi"
                };
                var resultUser = await userManager.CreateAsync(invite, "Password123!");
                if (resultUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(invite, "Utilisateur");
                }
            }
            if (!await context.Categories.AnyAsync())
            {
                await context.AddRangeAsync(
                    new Categorie { Nom = "Festival" },
                    new Categorie { Nom = "Culture" },
                    new Categorie { Nom = "Musique" },
                    new Categorie { Nom = "Anime" },
                    new Categorie { Nom = "Fitness" }
                );
                await context.SaveChangesAsync();
            }
        }

    }
}
