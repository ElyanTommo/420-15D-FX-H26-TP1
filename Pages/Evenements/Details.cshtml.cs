using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _420_15D_FX_H26_TP1.Pages.Evenements
{
    public class DetailsModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Utilisateur> _userManager;
        public DetailsModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context, IConfiguration configuration, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        public Evenement Evenement { get; set; } = default!;
        int nbParticipants;
        public string ImagePath { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements
                .Include(e => e.Categorie)
                .Include(e => e.Organisateur)
                .Include(e => e.Participations)
                .ThenInclude(p => p.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id == id);
            nbParticipants = _context.Evenements.Count();


            if (evenement == null)
            {
                return NotFound();

            }
            Evenement = evenement;
            ImagePath = _configuration["Images:PublicUrl"] ?? "/uploads/evenements/";

            return Page();
        }

        [Authorize]
        public async Task<IActionResult> OnPostInscriptionAsync(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var evenement = await _context.Evenements
                .Include(e => e.Participations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evenement == null) return NotFound();

            if (evenement.OrganisateurId == user.Id)
            {
                TempData["ErrorMessage"] = "Vous ne pouvez pas vous inscrire à votre propre événement.";
                return RedirectToPage(new { id });
            }

            var dejaInscrit = evenement.Participations
                .Any(p => p.UtilisateurId == user.Id);

            if (dejaInscrit)
            {
                TempData["ErrorMessage"] = "Vous êtes déjà inscrit à cet événement.";
                return RedirectToPage(new { id });
            }

            var participation = new Participation
            {
                EvenementId = evenement.Id,
                UtilisateurId = user.Id,
                DateInscription = DateTime.Now
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Vous êtes maintenant inscrit à cet événement!";
            return RedirectToPage(new { id });
        }

        [Authorize]
        public async Task<IActionResult> OnPostDesinscriptionAsync(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var participation = await _context.Participations
                .FirstOrDefaultAsync(p => p.EvenementId == id && p.UtilisateurId == user.Id);

            if (participation == null)
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas inscrit à cet événement.";
                return RedirectToPage(new { id });
            }

            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Vous vous êtes désinscrit de cet événement.";
            return RedirectToPage(new { id });
        }
    }
}
