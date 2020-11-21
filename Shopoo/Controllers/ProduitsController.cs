using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Shopoo.Models;
using System.Web;
using System.IO;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using System;

namespace Shopoo.Controllers
{
    public class ProduitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Produits
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await db.Produits.ToListAsync());
        }

        // GET: Produits/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Produit produit = await db.Produits.FindAsync(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // GET: Produits/Create
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.IdCategorie = new SelectList(db.Categories, "Id", "Libelle");
            return View();
        }

        // POST: Produits/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create([Bind(Include = "Id,Libelle,Prix,Description,QuantiteEnStock,MisEnVente,IdCategorie")] ProduitVM produit, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                //On utilise le Mapper (AutoMapper) pour gerer les relations
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ProduitVM, Produit>());
                var mapper = new Mapper(config);

                Produit p = mapper.Map<Produit>(produit);

                Categorie categorie = db.Categories.SingleOrDefault(c => c.Id == produit.IdCategorie);

                p.Categorie = new List<Categorie>();
                p.Categorie.Add(categorie);

                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/produits/"), fileName);
                    Image.SaveAs(path);
                    p.Image = fileName;
                }

                if (p.Image == null) p.Image = "product-default.png";

                Random random = new Random();

                p.UniqIdPanier = random.Next();

                db.Produits.Add(p);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(produit);
        }

        // GET: Produits/Edit/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Produit produit = await db.Produits.FindAsync(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategorie = new SelectList(db.Categories, "Id", "Libelle");
            return View(produit);
        }

        // POST: Produits/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Libelle,Prix,Description,QuantiteEnStock,MisEnVente")] Produit produit, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/images/produits/"), fileName);
                    Image.SaveAs(path);
                    produit.Image = fileName;
                }

                if (produit.Image == null) produit.Image = "product-default.png";

                db.Entry(produit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        // GET: Produits/Delete/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Produit produit = await db.Produits.FindAsync(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Produit produit = await db.Produits.FindAsync(id);
            db.Produits.Remove(produit);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
