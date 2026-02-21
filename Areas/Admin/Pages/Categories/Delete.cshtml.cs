using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Authorization;

namespace _420_15D_FX_H26_TP1.Areas.Admin.Pages.Categories
{
    [Authorize(Roles ="Admin")]
    public class DeleteModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;

        public DeleteModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Categorie Categorie { get; set; } = default!;

        public string? ErreurMessage { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories
                .Include(c => c.Evenements)
                .FirstOrDefaultAsync(m => m.Id == id);
           

            if (categorie == null)
            {
                return NotFound();
            }
            Categorie = categorie;
             return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var categorie = await _context.Categories
                .Include(c => c.Evenements)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categorie == null)
            {
                return NotFound();
            }
          
            var dateDuJour = DateTime.Now.Date;
            var evenementActifs = await _context.Evenements
                .Where(e => !e.EstArchive && e.DateFin.Date >= dateDuJour)
                .AnyAsync();

            if (evenementActifs)
            {


                ErreurMessage = "Impossible de supprimer cette catégorie:des évenements sont encore actif";
                Categorie = categorie;
                return Page();
                


            }
            categorie.EstArchive = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessageCatégorie"] = $"La catégorie '{Categorie.Nom}' a été supprimer avec succès.";
            return RedirectToPage("./Index");
        }
    }
}
