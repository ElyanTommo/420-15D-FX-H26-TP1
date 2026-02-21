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
    public class CreateModel : PageModel
    {
        private readonly _420_15D_FX_H26_TP1.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<Utilisateur> _userManager;


        public CreateModel(_420_15D_FX_H26_TP1.Data.ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment environment, UserManager<Utilisateur> userManager)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
            _userManager = userManager;
        }




        [BindProperty]
        public Evenement Evenement { get; set; }= new();

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public SelectList Categories { get; set; }

        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Evenement.OrganisateurId = user.Id;

            ViewData["CategorieId"] = new SelectList(
                await _context.Categories
                .Where(c => !c.EstArchive)
                .ToListAsync(),
                "Id",
                "Nom");

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {



            if (!ModelState.IsValid)
            {
                return Page();
            }


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Evenement.OrganisateurId = user.Id;

            if (!ModelState.IsValid)
            {
                ViewData["CategorieId"] = new SelectList(
                    await _context.Categories.ToListAsync(),
                    "Id",
                    "Nom");
                return Page();
            }

            if (Evenement.DateFin <= Evenement.DateDebut)
            {
                ModelState.AddModelError("Evenement.DateFin",
                    "La date de fin doit être après la date de début.");
                ViewData["CategorieId"] = new SelectList(
                    await _context.Categories.ToListAsync(),
                    "Id",
                    "Nom");
                return Page();
            }

            if (Evenement.DateDebut < DateTime.Now.Date)
            {
                ModelState.AddModelError("Evenement.DateDebut",
                    "La date de début ne peut pas être dans le passé.");
                ViewData["CategorieId"] = new SelectList(
                    await _context.Categories.ToListAsync(),
                    "Id",
                    "Nom");
                return Page();

            }
            var evenementExiste = await _context.Evenements
              .AnyAsync(e =>
                  e.Titre.ToLower() == Evenement.Titre.ToLower() &&
                  e.DateDebut.Date == Evenement.DateDebut.Date &&
                  e.Ville.ToLower() == Evenement.Ville.ToLower());

            if (evenementExiste)
            {
                ModelState.AddModelError(string.Empty,
                    "Un événement avec le même titre, date de début et ville existe déjà.");
                ViewData["CategorieId"] = new SelectList(
                    await _context.Categories.ToListAsync(),
                    "Id",
                    "Nom");
                return Page();
            }

            if (ImageFile != null)
            {
                if (ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageUpload",
                        "L'image ne doit pas dépasser 5 MB.");
                    ViewData["CategorieId"] = new SelectList(
                        await _context.Categories.ToListAsync(),
                        "Id",
                        "Nom");
                    return Page();
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageUpload",
                        "Seuls les fichiers JPG, PNG ou WEBP sont autorisés.");
                    ViewData["CategorieId"] = new SelectList(
                        await _context.Categories.ToListAsync(),
                        "Id",
                        "Nom");
                    return Page();
                }

                
                var uploadFolder = _configuration["Images:UploadPath"];
                var uploadPath = Path.Combine(_environment.WebRootPath, uploadFolder);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Evenement.Image = fileName;
            }

            _context.Evenements.Add(Evenement);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Votre évènement a été créer avec succès!";
            return RedirectToPage("./Index");


        }
    }
}
