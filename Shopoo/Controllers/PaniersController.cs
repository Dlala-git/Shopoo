using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Shopoo.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Shopoo.Utils;
using System.Data.Entity.Migrations;

namespace Shopoo.Controllers
{
    public class PaniersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IList<Produit> ProduitsPanier;
        private IList<Produit> SessionProduitPanier;

        // GET: Paniers
        public async Task<ActionResult> Index()
        {
            return View(await db.Commandes.ToListAsync());
        }

        [AllowAnonymous]
        public ActionResult AjouterAuPanier(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Produit produit = db.Produits.Find(id);

            if (Session["Panier"] == null)
            {
                ProduitsPanier = new List<Produit>();
                Session["Panier"] = ProduitsPanier;
            }

            SessionProduitPanier = (List<Produit>)Session["Panier"];
            SessionProduitPanier.Add(produit);
            Session["Panier"] = SessionProduitPanier;

            return Json(new { success = true, msg = "Produit ajouté au panier" }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult Voir()
        {
            SessionProduitPanier = (List<Produit>)Session["Panier"];
            ViewBag.TotalTTC = Calcul.CalculTotalTTC(SessionProduitPanier);
            return View("Voir", SessionProduitPanier);
        }

        [AllowAnonymous]
        public ActionResult SupprimerProduit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            SessionProduitPanier = (List<Produit>)Session["Panier"];
            Produit produit = SessionProduitPanier.Where(p => p.UniqIdPanier == id).SingleOrDefault();
            SessionProduitPanier.Remove(produit);
            ViewBag.TotalTTC = Calcul.CalculTotalTTC(SessionProduitPanier);

            return View("Voir", SessionProduitPanier);
        }

        [AllowAnonymous]
        public ActionResult ValiderPanier()
        {
            bool isLoggedIn = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            if (Utils.UtilisateurUtil.IsLoggeIn())
            {
                SessionProduitPanier = (List<Produit>)Session["Panier"];
                Panier panier = new Panier();
                panier.Produits = new List<Produit>();
                panier.Utilisateur = (Utilisateur)Session["Utilisateur"];

                for (int i = 0; i < SessionProduitPanier.Count(); i++)
                {
                    SessionProduitPanier[i].QuantiteEnStock--;
                    panier.Produits.Add(SessionProduitPanier[i]);
                }

                db.Paniers.Add(panier);
                db.SaveChanges();

                Random random = new Random();

                Commande commande = new Commande();
                commande.Utilisateur = panier.Utilisateur = (Utilisateur)Session["Utilisateur"];
                commande.Produits = new List<Produit>();
                commande.Produits = panier.Produits;
                commande.DateCommande = new DateTime().ToString();
                commande.NumeroDeCommande = random.Next();
                commande.TotalTTC = Calcul.CalculTotalTTC(SessionProduitPanier);

                db.Commandes.Add(commande);
                db.SaveChanges();

                return RedirectToAction("Details", "Commandes", new { id = commande.Id });
            }

            return RedirectToAction("Login", "Account");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
