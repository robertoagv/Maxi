using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.Controllers
{
    public class BoxesController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();

        // GET: Boxes
        public ActionResult Index()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var boxes = db.Boxes.Where(b => b.CompanyId == usuario.CompanyId).Include(b => b.Company);

            return View(boxes.OrderByDescending(b => b.BoxId).ToList());
        }

        // GET: Boxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            return View(box);
        }

        // GET: Boxes/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            var box = new Box { CompanyId = usuario.CompanyId, Fecha = DateTime.Today, usuario = usuario.NombreUser};

            //ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "nombresuc");

            return View(box);
        }

        // POST: Boxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Box box)
        {
            if (ModelState.IsValid)
            {
                db.Boxes.Add(box);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "nombresuc", box.CompanyId);

            return View(box);
        }

        // GET: Boxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "nombresuc", box.CompanyId);
            return View(box);
        }

        // POST: Boxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Box box)
        {
            if (ModelState.IsValid)
            {
                db.Entry(box).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "nombresuc", box.CompanyId);
            return View(box);
        }

        // GET: Boxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            return View(box);
        }

        // POST: Boxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Box box = db.Boxes.Find(id);
            db.Boxes.Remove(box);
            db.SaveChanges();
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
