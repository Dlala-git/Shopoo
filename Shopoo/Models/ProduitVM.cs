using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopoo.Models
{
    public class ProduitVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le libellé du produit est requis !")]
        public string Libelle { get; set; }

        [Required(ErrorMessage = "Le prix du produit est requis !")]
        public double Prix { get; set; }

        [Required(ErrorMessage = "La description du produit est requise !")]
        public string Description { get; set; }

        [DisplayName("Image produit")]
        public string Image { get; set; }

        [Required(ErrorMessage = "La quatitée de produits est requise !")]
        public int QuantiteEnStock { get; set; }

        [Required(ErrorMessage = "Veuillez indiquer si le produit est mis en vente")]
        public bool MisEnVente { get; set; }

        public virtual int IdCategorie { get; set; }

    }
}