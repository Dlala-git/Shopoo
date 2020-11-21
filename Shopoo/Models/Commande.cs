using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shopoo.Models
{
    public class Commande
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public string DateCommande { get; set; }

        public bool EstValide { get; set; }

        public double TotalTTC { get; set; }

        public int NumeroDeCommande { get; set; }

        public virtual Utilisateur Utilisateur { get; set; }

        public virtual ICollection<Produit> Produits { get; set; }
    }
}