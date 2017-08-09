using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMaxiFarmacia.Models;
using PagedList;
using WebMaxiFarmacia.classHelper;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CompaniesController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();

        
        public ActionResult Actualizar(int? id)
        {

          

            var productos = db.Products.Where(p => p.CompanyId == 1);
           
            

            foreach (var producto in productos)
            {
                var existe = db.Products.Where(p => p.CompanyId == id && p.Codigobarra == producto.Codigobarra).ToList();

                if (existe.Count() == 0)
                {
                    var newProdct = new Product
                    {
                        Codigobarra = producto.Codigobarra,
                        Nombreproducto = producto.Nombreproducto,
                        Descripcion = producto.Descripcion,
                        Preciocompra = producto.Precioventa,
                        Precioventa = 0,
                        Porcentaje = producto.Porcentaje,
                        CategoryId = producto.CategoryId,
                        SupplierId = 6,
                        CompanyId = (int)id
                    };

                    db.Products.Add(newProdct);

                }

            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Companies
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            return View(db.Companies.OrderByDescending(c => c.nombresuc).ToPagedList((int)page, 5));
        }
        [HttpPost]
        public ActionResult Index(string termino, int? page = null)
        {
            page = (page ?? 1);
            return View(db.Companies.Where(c => c.nombresuc == termino).OrderByDescending(c => c.nombresuc).ToPagedList((int)page, 10));
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    var productos = db.Products.Where(p => p.CompanyId == 1);

                    foreach (var producto in productos)
                    {
                        var newProdct = new Product
                        {
                            Codigobarra = producto.Codigobarra,
                            Nombreproducto = producto.Nombreproducto,
                            Descripcion = producto.Descripcion,
                            Preciocompra = producto.Precioventa,
                            Precioventa = 0,
                            Porcentaje = producto.Porcentaje,
                            CategoryId = producto.CategoryId,
                            SupplierId = 6,
                            CompanyId = company.CompanyId
                        };
                        db.Products.Add(newProdct);

                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Message);
                
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Message);
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);

            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, respuesta.Message);
           
            return View(company);
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
