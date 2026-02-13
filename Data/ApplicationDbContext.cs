using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _420_15D_FX_H26_TP1.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {


        public DbSet<Evenement> Evenements { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Participation> Participations { get; set; }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
