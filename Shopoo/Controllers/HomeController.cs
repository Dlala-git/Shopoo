using Microsoft.AspNet.Identity;
using Shopoo.Models;
using Shopoo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Shopoo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Utils.UtilisateurUtil.IsLoggeIn())
            {
                string IdIdentityFramework = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Utilisateur utilisateur = db.Utilisateurs.Where(u => u.IdIdentityFramework == IdIdentityFramework).FirstOrDefault<Utilisateur>();

                if (utilisateur != null)
                {
                    Session["Utilisateur"] = utilisateur;

                    Panier panier = db.Paniers.Where(p => p.Utilisateur.Id == utilisateur.Id).FirstOrDefault();

                    if (panier != null)
                    {
                        IList<Produit> ProduitsPanier = new List<Produit>();
                        Session["Panier"] = ProduitsPanier;
                    }
                }
            }

            return View(db.Produits.Where(p => p.MisEnVente == true).ToList<Produit>());
        }

        //Gestion des roles en cours
        [Authorize(Roles = Role.Admin)]
        public ActionResult Dashboard()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}