using System.ComponentModel.DataAnnotations;

namespace _420_15D_FX_H26_TP1.Models
{
    public class Categorie
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(25, ErrorMessage = "Le nom ne peut pas dépasser 25 caractères.")]
        public string? Nom { get; set; }
        public List<Evenement> Evenements { get; set; } = new();
    }
}
