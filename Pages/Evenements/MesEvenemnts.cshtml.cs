using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _420_15D_FX_H26_TP1.Pages.Evenements
{
    public class MesEvenemntsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Utilisateur> _userManager;


        public MesEvenemntsModel(ApplicationDbContext context,IConfiguration configuration,UserManager<Utilisateur> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }


        public IList<Evenement> EvenementsOrganises { get; set; } = new List<Evenement>();
        public IList<Participation> MesParticipations { get; set; } = new List<Participation>();

        public IList<Evenement> EvenementsArchives { get; set; } = new List<Evenement>();
        public IList<Participation> ParticipationsPassees { get; set; } = new List<Participation>();
        public string ImagePath { get; set; }
        public string NomUtilisateur { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            NomUtilisateur = $"{user.Prenom} {user.Nom}";

            EvenementsOrganises = await _context.Evenements
                .Where(e => e.OrganisateurId == user.Id && !e.EstArchive)
                .Include(e => e.Categorie)
                .Include(e => e.Participations)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            EvenementsArchives = await _context.Evenements
            .Where(e => e.OrganisateurId == user.Id && e.EstArchive)
            .Include(e => e.Categorie)
            .Include(e => e.Participations)
            .OrderByDescending(e => e.DateDebut)
            .ToListAsync();

            MesParticipations = await _context.Participations
                .Where(p => p.UtilisateurId == user.Id && !p.Evenement.EstArchive)
                .Include(p => p.Evenement)
                    .ThenInclude(e => e.Categorie)
                .Include(p => p.Evenement)
                    .ThenInclude(e => e.Organisateur)
                .OrderByDescending(p => p.Evenement.DateDebut)
                .ToListAsync();

    

            ImagePath = _configuration["Images:PublicUrl"] ?? "/uploads/evenements/";

            return Page();
        }
    }
}
