using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _420_15D_FX_H26_TP1.Pages.Evenements
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<Utilisateur> _userManager;

        public EditModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context,
            IConfiguration configuration, IWebHostEnvironment environment, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
            _userManager = userManager;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        public string? ImageActuelle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement =  await _context.Evenements
            .Include(e =>e.Categorie)
            .Include(e =>e.Organisateur)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }

            var userconnecter = await _userManager.GetUserAsync(User);
            if(userconnecter.Id != evenement.OrganisateurId)
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas l'organisateur de cet évènement";
                return RedirectToPage("./Index");
            }
           

           Evenement = evenement;
            ImageActuelle = evenement.Image;
           ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom",evenement.CategorieId);
           ViewData["NomOrganisateur"] = $"{evenement.Organisateur?.Nom}";
           return Page();
        }
        

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var evenementExistant = await _context.Evenements
               .Include(e => e.Organisateur)
               .FirstOrDefaultAsync(e => e.Id == Evenement.Id);
            if (evenementExistant == null)
            {
                return NotFound();
            }
            var userconnecter = await _userManager.GetUserAsync(User);
            if (userconnecter.Id != evenementExistant.OrganisateurId)
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas l'organisateur de cet évènement";
                return RedirectToPage("./Index");
            }

            if (Evenement.DateFin <= Evenement.DateDebut)
            {
                ModelState.AddModelError("Evenement.DateFin",
                    "La date de fin doit être après la date de début.");
                await RechargerViewData();
                return Page();
            }

            var evenementDuplique = await _context.Evenements
             .AnyAsync(e =>
                 e.Id != Evenement.Id &&
                 e.Titre.ToLower() == Evenement.Titre.ToLower() &&
                 e.DateDebut.Date == Evenement.DateDebut.Date &&
                 e.Ville.ToLower() == Evenement.Ville.ToLower());

            if (evenementDuplique)
            {
                ModelState.AddModelError(string.Empty,
                    "Un événement avec le même titre, date de début et ville existe déjà.");
                await RechargerViewData();
                return Page();
            }
            evenementExistant.Titre = Evenement.Titre;
            evenementExistant.Description = Evenement.Description;
            evenementExistant.CategorieId = Evenement.CategorieId;
            evenementExistant.Ville = Evenement.Ville;
            evenementExistant.CodePostal = Evenement.CodePostal;
            evenementExistant.Adresse = Evenement.Adresse;
            evenementExistant.DateDebut = Evenement.DateDebut;
            evenementExistant.DateFin = Evenement.DateFin;


            try
            {
                await _context.SaveChangesAsync();
                TempData["MessageSuccès"] = "L'évènement a été modifié avec succès!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvenementExists(Evenement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task RechargerViewData()
        {
            ViewData["CategorieId"] = new SelectList(
                await _context.Categories.ToListAsync(),
                "Id",
                "Nom",
                Evenement.CategorieId);

            var organisateur = await _context.Users.FindAsync(Evenement.OrganisateurId);
            ViewData["NomOrganisateur"] = organisateur != null
                ? $" {organisateur.Nom}"
                : "Inconnu";

            var evenementActuel = await _context.Evenements
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == Evenement.Id);
            ImageActuelle = evenementActuel?.Image;
        }

        private bool EvenementExists(int id)
        {
            return _context.Evenements.Any(e => e.Id == id);
        }
    }
}
