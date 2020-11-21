using System.ComponentModel.DataAnnotations;

namespace Shopoo.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        public string IdIdentityFramework { get; set; }

        [Required(ErrorMessage = "Votre Nom est requis !")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Votre prénom est requis !")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Votre Adresse est requise !")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Le Code Postal est requis !")]
        public string CodePostal { get; set; }

        [Required(ErrorMessage = "La Ville est requise !")]
        public string Ville { get; set; }

        [Required(ErrorMessage = "Merci d'indiquer votre numéro de téléphone")]
        public string TelephoneFixe { get; set; }

        public string TelephoneMobile { get; set; }


    }
}