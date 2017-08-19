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


        //public ActionResult movimientos(DateTime fecha, string term)
        //{
        //    var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
        //    var bodegafecha = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
        //    var inventoriesfecha = db.Inventories.Where(i => i.WarehouseId == bodegafecha.WarehouseId && i.FechaActualizada == fecha).Include(i => i.Product).Include(i => i.User).ToList();

        //    var salidas = db.SaleDetails.Where(s => s.)
        //}

        // GET: Inventories 
        public ActionResult Index()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => i.WarehouseId == bodega.WarehouseId).Include(i => i.Product).Include(i => i.User).ToList();

            

            return View(inventories);
        }
        [HttpPost]
        public ActionResult Index(string termino)
        {
            bool yesDate;
            DateTime buscarfecha;
            yesDate = DateTime.TryParse(termino, out buscarfecha);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (yesDate)
            {
                var bodegafecha = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
                var inventoriesfecha = db.Inventories.Where(i => i.WarehouseId == bodegafecha.WarehouseId && i.FechaActualizada == buscarfecha).Include(i => i.Product).Include(i => i.User).ToList();

                return View(inventoriesfecha);
            }

           
            var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => i.WarehouseId == bodega.WarehouseId && i.Product.Nombreproducto == termino).Include(i => i.Product).Include(i => i.User).ToList();

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
