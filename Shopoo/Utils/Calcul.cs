using Shopoo.Models;
using System;
using System.Collections.Generic;

namespace Shopoo.Utils
{
    public class Calcul
    {
        public static Double CalculTotalTTC(IList<Produit> produits)
        {
            double TotalTTC = 0;

            if (produits != null)
            {
                for (int i = 0; i < produits.Count; i++)
                {
                    TotalTTC += produits[i].Prix;
                }
            }

            return TotalTTC;
        }
    }
}