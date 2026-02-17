using System.ComponentModel.DataAnnotations;

namespace _420_15D_FX_H26_TP1.Models
{
    public class Evenement
    {

       public int Id { get; set; }
        public int CategorieId { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le titre ne peut pas dépasser 50 caractères.")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "La description est obligatoire.")]
        [StringLength(150, ErrorMessage = "La description ne peut pas dépasser 150 caractères.")]
        public string Description { get; set; }
        public Utilisateur? Organisateur { get; set; }
        public string? OrganisateurId { get; set; }
        public Categorie? Categorie { get; set; }

        [Required(ErrorMessage = "Le nom de la ville est obligatoire.")]
        [StringLength(20, ErrorMessage = "Le nom de la ville  ne peut pas dépasser 20 caractères.")]
        public string Ville { get; set; }

        [Required(ErrorMessage = "L'adresse de résidence est obligatoire.")]
        [StringLength(50, ErrorMessage = "L'adresse ne peut pas dépasser 50 caractères.")]
        public string Adresse { get; set; }


        [Required(ErrorMessage = "Le code postal est obligatoire.")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$",
          ErrorMessage = "Format de code postal invalide (ex: G1P 3T2).")]
        [StringLength(7, ErrorMessage = "Le code postal ne peut pas dépasser 7 caractères.")]
        public string CodePostal { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Format de date invalide.")]
        public DateTime DateDebut { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Format de date invalide.")]
        public DateTime DateFin { get; set; }

        public string? Image { get; set; }

        public bool EstArchive { get; set; } = false;
        public List<Participation> Participations { get; set; } = new();

    }
}
