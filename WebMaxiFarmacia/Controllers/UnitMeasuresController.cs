using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMaxiFarmacia.classHelper;
using WebMaxiFarmacia.Models;

namespace WebMaxiFarmacia.Controllers
{
    public class UnitMeasuresController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();

        // GET: UnitMeasures
        public ActionResult Index()
        {
            return View(db.UnitMeasures.ToList());
        }

        // GET: UnitMeasures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // GET: UnitMeasures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitMeasures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnitMeasure unitMeasure)
        {
            if (ModelState.IsValid)
            {
                db.UnitMeasures.Add(unitMeasure);
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
                return RedirectToAction("Index");
            }

            return View(unitMeasure);
        }

        // GET: UnitMeasures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // POST: UnitMeasures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnitMeasure unitMeasure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitMeasure).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
                return RedirectToAction("Index");
            }
            return View(unitMeasure);
        }

        // GET: UnitMeasures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // POST: UnitMeasures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            db.UnitMeasures.Remove(unitMeasure);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, respuesta.Message);
            return View(unitMeasure);
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
