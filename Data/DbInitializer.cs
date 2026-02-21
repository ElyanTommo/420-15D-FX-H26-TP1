//using _420_15D_FX_H26_TP1.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace _420_15D_FX_H26_TP1.Data
//{
//    public static class DbInitializer
//    {

//        public static async Task Initialize(ApplicationDbContext context,
//           UserManager<Utilisateur> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            if (!await roleManager.Roles.AnyAsync())
//            {
//                await roleManager.CreateAsync(new IdentityRole("Admin"));
//                await roleManager.CreateAsync(new IdentityRole("Utilisateur"));
//            }
//            if (!await userManager.Users.AnyAsync())
//            {
//                Utilisateur adminUser = new Utilisateur()
//                {
//                    UserName = "admin@cegepgarneau.ca",
//                    NomUtilisateur = "snake",
//                    Email = "admin@cegepgarneau.ca",
//                    Courriel = "admin@cegepgarneau.ca",
//                    Nom = "Admin",
//                    Prenom = "yann",
//                    Ville = "Sherbrooke",
//                    CodePostal = "G1M2V8",
//                    Adresse = "123 rue du marabout"

//                };
//                var resultAdmin = await userManager.CreateAsync(adminUser, "Password123!");
//                if (resultAdmin.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(adminUser, "Admin");
//                }

//                Utilisateur invite = new Utilisateur()
//                {
//                    UserName = "elyan@yahoo.com",
//                    NomUtilisateur= "WOLF",
//                    Email = "elyan@yahoo.com",
//                    Courriel = "elyan@yahoo.com",
//                    Nom = "ghost",
//                    Prenom = "loic",
//                    Ville = "Sherbrooke",
//                    CodePostal = "G1H5V7",
//                    Adresse = "123 rue du roi"
//                };
//                var resultUser = await userManager.CreateAsync(invite, "Password123!");
//                if (resultUser.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(invite, "Utilisateur");
//                }
//            }
//            if (!await context.Categories.AnyAsync())
//            {
//                await context.AddRangeAsync(
//                    new Categorie { Nom = "Festival" },
//                    new Categorie { Nom = "Culture" },
//                    new Categorie { Nom = "Musique" },
//                    new Categorie { Nom = "Anime" },
//                    new Categorie { Nom = "Fitness" }
//                );
//                await context.SaveChangesAsync();
//            }
//        }

//    }
//}




using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _420_15D_FX_H26_TP1.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<Utilisateur> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Utilisateur"));
            }

            
            var utilisateurs = new List<Utilisateur>();

            if (!await userManager.Users.AnyAsync())
            {
                Utilisateur adminUser = new Utilisateur()
                {
                    UserName = "admin@cegepgarneau.ca",
                    NomUtilisateur = "admin",
                    Email = "admin@cegepgarneau.ca",
                    Courriel = "admin@cegepgarneau.ca",
                    Nom = "Admin",
                    Prenom = "Admin",
                    Ville = "Sherbrooke",
                    CodePostal = "J1H 5N4",
                    Adresse = "475 Rue du Cégep",
                    EmailConfirmed = true
                };
                var resultAdmin = await userManager.CreateAsync(adminUser, "Password123!");
                if (resultAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                utilisateurs.Add(adminUser);

                var usersData = new[]
                {
                    new { Username = "alice.dupont@gmail.com", NomUtilisateur = "alice_d", Prenom = "Alice", Nom = "Dupont", Ville = "Montréal", CodePostal = "H2X 1Y4", Adresse = "123 Rue Saint-Denis" },
                    new { Username = "bob.martin@outlook.com", NomUtilisateur = "bob_m", Prenom = "Bob", Nom = "Martin", Ville = "Québec", CodePostal = "G1R 2L3", Adresse = "456 Grande Allée" },
                    new { Username = "carla.tremblay@yahoo.fr", NomUtilisateur = "carla_t", Prenom = "Carla", Nom = "Tremblay", Ville = "Laval", CodePostal = "H7T 2R3", Adresse = "789 Boulevard Cartier" },
                    new { Username = "david.roy@hotmail.com", NomUtilisateur = "david_r", Prenom = "David", Nom = "Roy", Ville = "Gatineau", CodePostal = "J8X 2W8", Adresse = "321 Rue Principale" },
                    new { Username = "emma.gagnon@gmail.com", NomUtilisateur = "emma_g", Prenom = "Emma", Nom = "Gagnon", Ville = "Sherbrooke", CodePostal = "J1H 2Y3", Adresse = "654 Rue King" },
                    new { Username = "felix.cote@outlook.com", NomUtilisateur = "felix_c", Prenom = "Félix", Nom = "Côté", Ville = "Trois-Rivières", CodePostal = "G8Z 4M3", Adresse = "987 Rue des Forges" }
                };

                foreach (var userData in usersData)
                {
                    Utilisateur user = new Utilisateur()
                    {
                        UserName = userData.Username,
                        NomUtilisateur = userData.NomUtilisateur,
                        Email = userData.Username,
                        Courriel = userData.Username,
                        Nom = userData.Nom,
                        Prenom = userData.Prenom,
                        Ville = userData.Ville,
                        CodePostal = userData.CodePostal,
                        Adresse = userData.Adresse,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(user, "Password123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Utilisateur");
                    }
                    utilisateurs.Add(user);
                }
            }
            else
            {
                utilisateurs = await userManager.Users.ToListAsync();
            }

           
            if (!await context.Categories.AnyAsync())
            {
                await context.Categories.AddRangeAsync(
                    new Categorie { Nom = "Festival", EstArchive = false },
                    new Categorie { Nom = "Culture", EstArchive = false },
                    new Categorie { Nom = "Musique", EstArchive = false },
                    new Categorie { Nom = "Anime", EstArchive = false },
                    new Categorie { Nom = "Fitness", EstArchive = false },
                    new Categorie { Nom = "Technologie", EstArchive = false }
                );
                await context.SaveChangesAsync();
            }

           
            if (!await context.Evenements.AnyAsync())
            {
                var categories = await context.Categories.ToListAsync();
                var random = new Random();

                var evenementsData = new[]
                {
                    new { Titre = "Festival d'été de Québec 2026", Description = "Le plus grand festival de musique extérieur au Canada avec des artistes internationaux.", Categorie = "Festival", Ville = "Québec", CodePostal = "G1R 4S7", Adresse = "Place D'Youville", JoursDansLeFutur = 60 },
                    new { Titre = "Igloofest Montréal", Description = "Festival de musique électronique en plein air dans le Vieux-Port de Montréal.", Categorie = "Festival", Ville = "Montréal", CodePostal = "H2Y 2E2", Adresse = "Quai Jacques-Cartier", JoursDansLeFutur = 15 },

                    new { Titre = "Exposition Japon : Arts et Traditions", Description = "Découvrez l'art japonais traditionnel et contemporain au Musée des Beaux-Arts.", Categorie = "Culture", Ville = "Montréal", CodePostal = "H3G 1M8", Adresse = "1380 Rue Sherbrooke Ouest", JoursDansLeFutur = 10 },
                    new { Titre = "Soirée Cinéma Indépendant", Description = "Projection de films québécois indépendants suivie d'une discussion avec les réalisateurs.", Categorie = "Culture", Ville = "Sherbrooke", CodePostal = "J1H 5N4", Adresse = "Cinéma du Cégep", JoursDansLeFutur = 25 },

                    new { Titre = "Concert Symphonique de Noël", Description = "L'Orchestre Symphonique de Québec présente les grands classiques de Noël.", Categorie = "Musique", Ville = "Québec", CodePostal = "G1R 5B3", Adresse = "Grand Théâtre de Québec", JoursDansLeFutur = 50 },
                    new { Titre = "Soirée Jazz au Upstairs", Description = "Concert intimiste de jazz avec des musiciens locaux renommés.", Categorie = "Musique", Ville = "Montréal", CodePostal = "H2X 1N9", Adresse = "1254 Rue Mackay", JoursDansLeFutur = 18 },

                    new { Titre = "Otakuthon 2026", Description = "Convention d'anime, manga et culture japonaise avec cosplay, panels et invités spéciaux.", Categorie = "Anime", Ville = "Montréal", CodePostal = "H3C 3R4", Adresse = "Palais des congrès", JoursDansLeFutur = 45 },
                    new { Titre = "Projection Ghibli Marathon", Description = "Marathon des films Studio Ghibli sur grand écran. Cosplay encouragé!", Categorie = "Anime", Ville = "Laval", CodePostal = "H7T 2H5", Adresse = "Cinéma Guzzo Marché Central", JoursDansLeFutur = 30 },

                    new { Titre = "Yoga en Plein Air - Parc du Mont-Royal", Description = "Séance de yoga gratuite au lever du soleil avec vue sur Montréal.", Categorie = "Fitness", Ville = "Montréal", CodePostal = "H2W 1S6", Adresse = "Parc du Mont-Royal", JoursDansLeFutur = 12 },
                    new { Titre = "Marathon de Sherbrooke", Description = "Course de 42 km à travers la ville de Sherbrooke. Demi-marathon et 5 km aussi disponibles.", Categorie = "Fitness", Ville = "Sherbrooke", CodePostal = "J1H 1Z1", Adresse = "Centre-ville de Sherbrooke", JoursDansLeFutur = 40 },

                    new { Titre = "Hackathon IA Québec", Description = "48 heures de programmation intensive sur l'intelligence artificielle et le machine learning.", Categorie = "Technologie", Ville = "Québec", CodePostal = "G1V 0A6", Adresse = "Université Laval", JoursDansLeFutur = 35 },
                    new { Titre = "Web Summit Montréal", Description = "Conférence sur les dernières technologies web, mobile et cloud computing.", Categorie = "Technologie", Ville = "Montréal", CodePostal = "H3B 1A1", Adresse = "Centre des Sciences", JoursDansLeFutur = 55 }
                };

                var evenements = new List<Evenement>();

                foreach (var eventData in evenementsData)
                {
                    var categorie = categories.First(c => c.Nom == eventData.Categorie);
                    var organisateur = utilisateurs[random.Next(utilisateurs.Count)];

                    var dateDebut = DateTime.Now.AddDays(eventData.JoursDansLeFutur);
                    var dateFin = dateDebut.AddDays(random.Next(1, 4)); 

                    evenements.Add(new Evenement
                    {
                        Titre = eventData.Titre,
                        Description = eventData.Description,
                        CategorieId = categorie.Id,
                        OrganisateurId = organisateur.Id,
                        Ville = eventData.Ville,
                        CodePostal = eventData.CodePostal,
                        Adresse = eventData.Adresse,
                        DateDebut = dateDebut,
                        DateFin = dateFin,
                        EstArchive = false,
                        Image = "default.png" 
                    });
                }

                context.Evenements.AddRange(evenements);
                await context.SaveChangesAsync();
            }

            
            if (!context.Participations.Any())
            {
                var evenements = await context.Evenements.ToListAsync();
                var random = new Random();
                var participations = new List<Participation>();

                foreach (var utilisateur in utilisateurs)
                {
                    var nombreParticipations = random.Next(2, 5);
                    var evenementsDisponibles = evenements
                        .Where(e => e.OrganisateurId != utilisateur.Id) 
                        .ToList();

                    if (evenementsDisponibles.Any())
                    {
                        var evenementsChoisis = evenementsDisponibles
                            .OrderBy(x => random.Next())
                            .Take(Math.Min(nombreParticipations, evenementsDisponibles.Count))
                            .ToList();

                        foreach (var evenement in evenementsChoisis)
                        {
                            participations.Add(new Participation
                            {
                                EvenementId = evenement.Id,
                                UtilisateurId = utilisateur.Id,
                                DateInscription = DateTime.Now.AddDays(-random.Next(1, 15))
                            });
                        }
                    }
                }

                context.Participations.AddRange(participations);
                await context.SaveChangesAsync();
            }
        }
    }
}


