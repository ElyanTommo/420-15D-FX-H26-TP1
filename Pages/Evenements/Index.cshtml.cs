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
    public class IndexModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;



        public IndexModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IList<Evenement> Evenement { get;set; } = default!;
        public string ImagePath { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            //Evenement = await _context.Evenements
            //    .Include(e => e.Categorie)
            //    .Include(e => e.Organisateur).ToListAsync();

            Evenement = await _context.Evenements
                .Include(e => e.Categorie)
                .Include(e => e.Organisateur)
                 .OrderByDescending(e => e.DateDebut)
                 .ToListAsync();

            ImagePath = _configuration["Images:PublicUrl"] ?? "/uploads/events/";
        }
    }
}
