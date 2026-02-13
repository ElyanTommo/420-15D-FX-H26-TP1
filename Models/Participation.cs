namespace _420_15D_FX_H26_TP1.Models
{
    public class Participation
    {
        public int Id { get; set; }

        public int EvenementId { get; set; }
        public Evenement? Evenement { get; set; }

        public string UtilisateurId { get; set; } = "";
        public Utilisateur? Utilisateur { get; set; }


        public DateTime DateInscription { get; set; } = DateTime.Now;






    }
}
