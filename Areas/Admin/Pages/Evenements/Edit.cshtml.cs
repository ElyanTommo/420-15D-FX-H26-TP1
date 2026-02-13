using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;

namespace _420_15D_FX_H26_TP1.Areas.Admin.Pages.Evenements
{
    public class EditModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;

        public EditModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement =  await _context.Evenements.FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }
            Evenement = evenement;
           ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Id");
           ViewData["OrganisateurId"] = new SelectList(_context.Users, "Id", "Id");
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

            _context.Attach(Evenement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        private bool EvenementExists(int id)
        {
            return _context.Evenements.Any(e => e.Id == id);
        }
    }
}
