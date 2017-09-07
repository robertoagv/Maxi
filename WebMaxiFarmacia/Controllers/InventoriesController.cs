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
    public class InventoriesController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();

        public JsonResult buscarProductojq(string term)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var finded = db.Products.Where(p => p.CompanyId == usuario.CompanyId && p.Nombreproducto.StartsWith(term)).Select(p => p.Nombreproducto).ToList();

            return Json(finded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult movimientos()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var inventario = db.Inventories.Where(i => i.UltimoAdd < 0).ToList();
            var ventasmas = db.SaleDetails.Where(v => v.SaleId < 0).FirstOrDefault();
            ViewBag.mensaje = "Debe buscar por nombre o codigo de producto";
            ViewBag.color = "azul";
            ViewBag.sucursal = "Sucursal: " + usuario.Company.nombresuc;
            return View(inventario);
        }

        [HttpPost]
        public ActionResult movimientos(string term)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var bodegafecha = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();


            long codigo;
            bool silong = long.TryParse(term, out codigo);
            if (silong)
            {
                var inventoriesCodigo = db.Inventories.Where(i => i.WarehouseId == bodegafecha.WarehouseId && i.Product.Codigobarra == codigo).Include(i => i.Product).Include(i => i.User).ToList();
                var ventasCodigo = (from s in db.Sales.ToList()
                                                join sd in db.SaleDetails.ToList()
                                                on s.SaleID equals sd.SaleId
                                                where s.CompanyId == usuario.CompanyId && sd.Product.Codigobarra == codigo
                                                orderby sd.SaleId descending
                                                select new saledetailr
                                                {
                                                    Codigo = sd.Product.Codigobarra,
                                                    descripcion = sd.Descriptionpro,
                                                    cantidad = sd.Cantidad,
                                                    fecha = s.Fechavta,
                                                    usuario = s.Users.NombreUser
                                                }).FirstOrDefault();

                ViewBag.ventas= ventasCodigo;
                ViewBag.sucursal = "Sucursal: " + usuario.Company.nombresuc;
                return View(inventoriesCodigo);
            }

           

            var ventafecha = (from s in db.Sales.ToList()
                              join sd in db.SaleDetails.ToList()
                              on s.SaleID equals sd.SaleId
                              where s.CompanyId == usuario.CompanyId && sd.Descriptionpro == term
                              orderby sd.SaleId descending
                              select new saledetailr
                              {
                                  Codigo = sd.Product.Codigobarra,
                                  descripcion = sd.Descriptionpro,
                                  cantidad = sd.Cantidad,
                                  fecha = s.Fechavta,
                                  usuario = s.Users.NombreUser
                              }).FirstOrDefault();
            ViewBag.ventas = ventafecha;
            if (ventafecha != null)
            {
                codigo = ventafecha.Codigo;
                
            }
            else
            {
                codigo = 0;
                ViewBag.color = "rojo";
                ViewBag.mensaje = "El producto no tiene inventario o ninguna compra realizada.";
            }

            
            var inventoriesfecha = db.Inventories.Where(i => i.WarehouseId == bodegafecha.WarehouseId && i.Product.Codigobarra == codigo).Include(i => i.Product).Include(i => i.User).ToList();
            ViewBag.sucursal = "Sucursal: " + usuario.Company.nombresuc;
            return View(inventoriesfecha);
        }

        // GET: Inventories 
        public ActionResult Index()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            //var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => /*i.WarehouseId == bodega.WarehouseId &&*/ i.inventoryId == 0).Include(i => i.Product).Include(i => i.User).ToList();
            ViewBag.sucursal = "Sucursal: " + usuario.Company.nombresuc;


            return View(inventories);
        }
        [HttpPost]
        public ActionResult Index(string term)
        {
            bool yesLong;
            long codigo; 
            yesLong = long.TryParse(term, out codigo);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (yesLong)
            {
                var bodegafecha = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
                var inventoriesfecha = db.Inventories.Where(i => i.WarehouseId == bodegafecha.WarehouseId && i.Product.Codigobarra == codigo).Include(i => i.Product).Include(i => i.User).ToList();
                ViewBag.sucursal = usuario.Company.nombresuc;
                return View(inventoriesfecha);
            }

            
            var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => i.WarehouseId == bodega.WarehouseId && i.Product.Nombreproducto.StartsWith(term)).Include(i => i.Product).Include(i => i.User).ToList();
            ViewBag.sucursal = usuario.Company.nombresuc;

            return View(inventories);
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Nombreproducto");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Nombre");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Nombreproducto", inventory.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser", inventory.UserId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Nombre", inventory.WarehouseId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Nombreproducto", inventory.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser", inventory.UserId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Nombre", inventory.WarehouseId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "inventoryId,WarehouseId,ProductId,Existencia,FechaCreada,FechaActualizada,UserId")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Nombreproducto", inventory.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser", inventory.UserId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Nombre", inventory.WarehouseId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, respuesta.Message);

            return View(inventory);
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
