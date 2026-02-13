using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;

namespace _420_15D_FX_H26_TP1.Areas.Admin.Pages.Evenements
{
    public class CreateModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;

        public CreateModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Id");
        ViewData["OrganisateurId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Evenements.Add(Evenement);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
