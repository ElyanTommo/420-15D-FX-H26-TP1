using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;

namespace _420_15D_FX_H26_TP1.Pages.Evenements
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public Evenement Evenement { get; set; }

        public SelectList Categories { get; set; }

        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom");
        ViewData["OrganisateurId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _context.Evenements.Add(Evenement);
            await _context.SaveChangesAsync();
            TempData["message"] = "L'évènement a été crée avec succès! :D";
            return RedirectToPage("./Index");
        }
    }
}
