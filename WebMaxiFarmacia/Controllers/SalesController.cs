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
using PagedList;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class SalesController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();

        public ActionResult AgregarProducto()
        {
            var viewpro = new addProductView{ Codigobarra = -1};

            return View(viewpro);
        }

        [HttpPost]
        public ActionResult AgregarProducto(long? barcodigo)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var producto = db.Products.Where(p => p.CompanyId == usuario.CompanyId && p.Codigobarra == barcodigo).FirstOrDefault();

            if (producto ==  null)
            {
                addProductView viewproductonotfind = new addProductView
                {
                   
                    Codigobarra = 0
                   
                };

                return View(viewproductonotfind);
            }

            ViewBag.producto = producto;
            ViewBag.Existenciascero = " ";
            var viewproductofind = new addProductView
            {
                ProductId = producto.ProductId,
                Codigobarra = producto.Codigobarra,
                Nombreproducto = producto.Nombreproducto,
                Existencia = producto.Existencia,
                Precioventa = producto.Precioventa,

            };

            return View(viewproductofind);
        }

        [HttpPost]
        public ActionResult AgregarProductoFind(addProductView pview)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (pview.Cantidad > pview.Existencia || pview.Cantidad == 0)
            {
                
                return RedirectToAction("AgregarProducto", "Sales");

            }

            ModelState.AddModelError(string.Empty, "La cantidad que desea agregar es mayor a la existencia o igual a 0.");

            if (ModelState.IsValid)
                {
                
                    var saleYesExistInDetails = db.SaleDetailTmps.Where(sed => sed.NombreUsuario == User.Identity.Name && sed.ProductId == pview.ProductId).FirstOrDefault();
                    if (saleYesExistInDetails == null)
                    {
                        saleYesExistInDetails = new SaleDatilsTmp
                        {
                            NombreUsuario = User.Identity.Name,
                            ProductId = pview.ProductId,
                            Descriptionpro = pview.Nombreproducto,
                            Precio = pview.Precioventa,
                            Cantidad = pview.Cantidad,
                        };

                        db.SaleDetailTmps.Add(saleYesExistInDetails);
                    }
                    else
                    {
                        saleYesExistInDetails.Cantidad += pview.Cantidad;
                        db.Entry(saleYesExistInDetails).State = EntityState.Modified;
                    }

                    var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                    if (respuesta.Succeeded)
                    {
                        return RedirectToAction("Create");
                    }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            return View(pview);
        }

        public ActionResult DeleteProductList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var saleDetailTmpFind = db.SaleDetailTmps.Where(sdt => sdt.NombreUsuario == User.Identity.Name && sdt.ProductId == id).FirstOrDefault();
            if (saleDetailTmpFind == null)
            {
                return HttpNotFound();
            }

            db.SaleDetailTmps.Remove(saleDetailTmpFind);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Create");
            }
            ModelState.AddModelError(string.Empty, respuesta.Message);

            return View(saleDetailTmpFind);
        }

       
        // GET: Sales
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var sales = db.Sales.Where(s => s.CompanyId == usuario.CompanyId && s.Fechavta == DateTime.Today).Include(s => s.Users).OrderByDescending(s => s.SaleID);
            return View(sales.ToPagedList((int)page, 5));
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Where(s => s.SaleID == id).Include(s => s.SaleDetails).FirstOrDefault();
            if (sale== null)
            {
                return HttpNotFound();
            }

            sale.Detalles = db.SaleDetails.Where(sd => sd.SaleId == sale.SaleID).ToList();

            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserId = usuario.NombreUser;

            var view = new NewSaleView()
            {
                Fechavta = DateTime.Today,
                UserId = usuario.UserId,
                Detalles = db.SaleDetailTmps.Where(sdt => sdt.NombreUsuario == User.Identity.Name).ToList()
            };

            return View(view);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewSaleView newSaleView)
        {
            if (ModelState.IsValid)
            {
                var respuesta = MovementsHelper.newSale(newSaleView, User.Identity.Name);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Message);
                
            }

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            ViewBag.UserId = usuario.NombreUser;

            var view = new NewSaleView()
            {
                Fechavta = DateTime.Today,
                UserId = usuario.UserId,
                Detalles = db.SaleDetailTmps.Where(sdt => sdt.NombreUsuario == User.Identity.Name).ToList()
            };
            return View(view);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser", sale.UserId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaleID,Nombrecte,Fechavta,UserId")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "NombreUser", sale.UserId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, respuesta.Message);

            return View(sale);
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
