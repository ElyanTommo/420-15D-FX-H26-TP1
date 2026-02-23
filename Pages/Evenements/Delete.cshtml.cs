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
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly IConfiguration _configuration;

        public DeleteModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context, UserManager<Utilisateur> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null) 
            { 
                return NotFound();
            }
            var userConnecter = await _userManager.GetUserAsync(User);
            if (userConnecter.Id != evenement.OrganisateurId)
            {
                TempData["ErrorMessage"] = "Seul l'organisateur peut  supprimer son évènement";
                return RedirectToPage("./Index");
            }

            Evenement = evenement;
            return Page();

            
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evenement == null)
            {
                return NotFound();
            }

            var userConnecter = await _userManager.GetUserAsync(User);
            if (userConnecter.Id != evenement.OrganisateurId)
            {
                TempData["ErrorMessage"] = "Seul l'organisateur peut  supprimer son évènement";
                return RedirectToPage("./Index");
            }
            if (evenement != null)
            {
                evenement.EstArchive=true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "L'événement a été supprimé avec succès.";

            }

            return RedirectToPage("./Index");
        }
    }
}
