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
using PagedList;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class ProductsController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboAll = new cboAll();
       

        // GET: Products
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1); 

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = db.Products.Where(p => p.CompanyId == usuario.CompanyId).Include(p => p.Category).Include(p => p.Company).Include(p => p.Supplier).OrderByDescending(p => p.ProductId);
            return View(products.ToPagedList((int)page, 5));
        }


        [HttpPost]
        public ActionResult Index(string termino, int? page = null)
        {
            page = (page ?? 1);
            bool longsi;
            long barcodigo;
            string namepro;

            longsi = long.TryParse(termino, out barcodigo);

            if (longsi)
            {
                if (barcodigo > 0)
                {
                    var usuarioi = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
                    var producto = db.Products.Where(p => p.Codigobarra == barcodigo && p.CompanyId == usuarioi.CompanyId).OrderBy(p => p.ProductId);
                    //TODO: agregar aqui los usuarios a los que p
                    return View(producto.ToPagedList((int)page, 5));
                }
                else
                {
                    return View();
                }
            }
            else
            {
                namepro = termino;
                var usuarioi = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
                var producto = db.Products.Where(p =>  p.CompanyId == usuarioi.CompanyId && p.Nombreproducto.StartsWith(namepro)).OrderBy(p => p.ProductId);
                //TODO: agregar aqui los usuarios a los que p
                return View(producto.ToPagedList((int)page, 10));
            }
        }

        public ActionResult comprar(int? id)
        {

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();

            var inventario = db.Inventories.Where(i => i.ProductId == id && i.WarehouseId == bodega.WarehouseId).FirstOrDefault();

            if (inventario == null)
            {
                var inventarioget = new Inventory { WarehouseId = bodega.WarehouseId, ProductId = id, Existencia = 0 };
                ViewBag.inventario = inventarioget;
                return PartialView(inventarioget);
            }

            ViewBag.inventario = inventario;
           
            return PartialView(inventario);
        }

        [HttpPost]
        public ActionResult comprar(int idbodega, int idinventario, int idproducto, int newcant)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var inventario = db.Inventories.Find(idinventario);

            if (inventario == null)
            {
               var  inventarionew = new Inventory {
                        WarehouseId = idbodega,
                        ProductId = idproducto,
                        Existencia = newcant,
                        FechaCreada = DateTime.Today,
                        FechaActualizada = DateTime.Today,
                        UserId = usuario.UserId
                };

                db.Inventories.Add(inventarionew);
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            if (newcant <= 0)
            {
                return RedirectToAction("Details/" + idproducto, "Products");
            }

            ModelState.AddModelError(string.Empty, "Error, la cantidad es 0, o mayor a la Existencia."); 

            var catidad = (from i in db.Inventories
                           where i.ProductId == idproducto
                           select i.Existencia).FirstOrDefault();

            var oldexist = int.Parse(catidad.ToString());
            var newexist = oldexist + newcant;

            inventario.Existencia = newexist;
            inventario.FechaActualizada = DateTime.Today;
            inventario.UserId = usuario.UserId;

            var respueta = ChangeValidationHelperDb.ChangeDb(db);
            if (respueta.Succeeded)
            {
                ViewBag.inventario = inventario;

                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, respueta.Message);

            return View(inventario);
            
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
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion");
            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre");

            db.Products.Where(p => p.CompanyId == usuario.CompanyId).FirstOrDefault();
            var producto = new Product { CompanyId = usuario.CompanyId };

            return View(producto);
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
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "El producto con este Coidigo de Barras ya Existe.");
            }

            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion", product.CategoryId);
            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
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
            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }
       
            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion", product.CategoryId);
            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", product.CompanyId);
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
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, respuesta.Message);

            return View(product);
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
