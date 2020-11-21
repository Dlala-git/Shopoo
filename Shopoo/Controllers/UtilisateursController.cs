using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Shopoo.Models;
using Microsoft.AspNet.Identity;

namespace Shopoo.Controllers
{
    public class UtilisateursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Utilisateurs
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await db.Utilisateurs.ToListAsync());
        }

        // GET: Utilisateurs/Details/5
        //[Authorize(Roles = "Admin,Client")]
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nom,Prenom,Adresse,CodePostal,Ville,TelephoneFixe,TelephoneMobile")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                utilisateur.IdIdentityFramework = System.Web.HttpContext.Current.User.Identity.GetUserId();
                db.Utilisateurs.Add(utilisateur);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        //[Authorize(Roles = "Client")]
        [AllowAnonymous]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nom,Prenom,Adresse,CodePostal,Ville,TelephoneFixe,TelephoneMobile")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        //[Authorize(Roles = "Client")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = await db.Utilisateurs.FindAsync(id);
            db.Utilisateurs.Remove(utilisateur);
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
