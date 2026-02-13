using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;

namespace _420_15D_FX_H26_TP1.Pages.Evenements
{
    public class DeleteModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;

        public DeleteModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context)
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

            var evenement = await _context.Evenements.FirstOrDefaultAsync(m => m.Id == id);

            if (evenement is not null)
            {
                Evenement = evenement;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.FindAsync(id);
            if (evenement != null)
            {
                Evenement = evenement;
                _context.Evenements.Remove(Evenement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
