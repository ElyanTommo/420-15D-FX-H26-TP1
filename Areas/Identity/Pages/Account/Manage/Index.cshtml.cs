// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using _420_15D_FX_H26_TP1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _420_15D_FX_H26_TP1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;

        public IndexModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Le prénom est obligatoire.")]
            [StringLength(50)]
            [Display(Name = "Prénom")]
            public string Prenom { get; set; }

            [Required(ErrorMessage = "Le nom est obligatoire.")]
            [StringLength(50)]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
            [StringLength(50)]
            [Display(Name = "Nom d'utilisateur")]
            public string NomUtilisateur { get; set; }

            [Required(ErrorMessage = "L'adresse est obligatoire.")]
            [StringLength(200)]
            [Display(Name = "Adresse")]
            public string Adresse { get; set; }

            [Required(ErrorMessage = "La ville est obligatoire.")]
            [StringLength(100)]
            [Display(Name = "Ville")]
            public string Ville { get; set; }

            [Required(ErrorMessage = "Le code postal est obligatoire.")]
            [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$",
                ErrorMessage = "Code postal invalide (ex: H2X 1Y4).")]
            [StringLength(7)]
            [Display(Name = "Code postal")]
            public string CodePostal { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Utilisateur user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            //Input = new InputModel
            //{
            //    PhoneNumber = phoneNumber
            //};
            Input = new InputModel
            {
                Prenom = user.Prenom,
                Nom = user.Nom,
                NomUtilisateur = user.NomUtilisateur,
                Adresse = user.Adresse,
                Ville = user.Ville,
                CodePostal = user.CodePostal
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Prenom = Input.Prenom;
            user.Nom = Input.Nom;
            user.NomUtilisateur = Input.NomUtilisateur;
            user.Adresse = Input.Adresse;
            user.Ville = Input.Ville;
            user.CodePostal = Input.CodePostal;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Erreur lors de la mise à jour du profil.";
                return RedirectToPage();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Votre profil a été mis à jour avec succès.";
            return RedirectToPage();
        }
    }
}
