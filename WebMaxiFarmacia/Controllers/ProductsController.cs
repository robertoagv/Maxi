using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMaxiFarmacia.Models;
using WebMaxiFarmacia.classHelper;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin, User")]
    public class ProductsController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboAll = new cboAll();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Company).Include(p => p.Supplier);
            return View(products.ToList());
        }


        [HttpPost]
        public ActionResult Index(long? barcodigo)
        {
            if (barcodigo > 0)
            {
                var producto = db.Products.Where(p => p.Codigobarra == barcodigo).ToList();
                return View(producto);
            }
            else
            {

                return View();
            }


        }

        public ActionResult comprar(int? id)
        {
            var inventario = db.Inventories.Find(id);
  
            ViewBag.inventario = inventario;
           
            return PartialView(inventario);
        }

        [HttpPost]
        public ActionResult comprar(int idbodega, int idinventario, int idproducto, int newcant)
        {
            var catidad = (from i in db.Inventories
                           where i.ProductId == idproducto
                           select i.Existencia).FirstOrDefault();

            var oldexist = int.Parse(catidad.ToString());
            var newexist = oldexist + newcant;

            var inventario = db.Inventories.Find(idinventario);
            inventario.Existencia = newexist;
            db.SaveChanges();

            ViewBag.inventario = inventario;

            return RedirectToAction("Index");
        }



        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion");
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion", product.CategoryId);
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion", product.CategoryId);
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Codigobarra,Nombreproducto,Descripcion,Preciocompra,Precioventa,CategoryId,SupplierId,CompanyId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion", product.CategoryId);
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
