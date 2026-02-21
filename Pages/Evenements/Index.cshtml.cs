using _420_15D_FX_H26_TP1.Data;
using _420_15D_FX_H26_TP1.Models;
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


        public string? eventRechercher { get; set; }

        public int? CategorieId { get; set; }

        public SelectList Categories { get; set; } = default!;
        public DateTime? dateDebut { get; set; }
        public DateTime? dateFin { get; set; }

        public async Task OnGetAsync(string? eventRechercher, int? categorieId, DateTime? dateDebut, DateTime? dateFin)    
        {

            var dateActuel = DateTime.Now.Date;

            var evenementsTermine = await _context.Evenements
                .Where(e => !e.EstArchive && e.DateFin.Date < dateActuel)
                .ToListAsync();
            if (evenementsTermine.Any()) 
            { 
                foreach (var e in evenementsTermine)
                {
                    e.EstArchive = true;
                }
                await _context.SaveChangesAsync();
            }


            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
            this.eventRechercher = eventRechercher;
            this.CategorieId = categorieId;

            Categories = new SelectList(
               await _context.Categories
                   .Where(c => !c.EstArchive)
                   .OrderBy(c => c.Nom)
                   .ToListAsync(),
               "Id",
               "Nom");

            var lesEvenements = await _context.Evenements
                .Where(e => !e.EstArchive)
                .Include(e => e.Categorie)
                .Include(e => e.Organisateur)
                .Include(e => e.Participations)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(eventRechercher))
            {
                lesEvenements = lesEvenements
                    .Where(e =>
                        e.Titre.Contains(eventRechercher) ||
                        e.Description.Contains(eventRechercher))
                    .ToList();
            }

            if (categorieId.HasValue && categorieId.Value > 0)
            {
                lesEvenements = lesEvenements
                    .Where(e => e.CategorieId == categorieId.Value)
                    .ToList();
            }

            if (dateDebut.HasValue)
            {
                lesEvenements = lesEvenements
                    .Where(e => e.DateDebut >= dateDebut.Value)
                    .ToList();
            }

            if (dateFin.HasValue)
            {
                lesEvenements = lesEvenements
                    .Where(e => e.DateFin <= dateFin.Value)
                    .ToList();
            }

            Evenement = lesEvenements;

            ImagePath = _configuration["Images:PublicUrl"] ?? "/uploads/events/";
        }
    }
}
