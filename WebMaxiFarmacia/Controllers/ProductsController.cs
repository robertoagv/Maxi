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
using System.IO;
using ExcelDataReader;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, User")]
    public class ProductsController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboAll = new cboAll();

        public JsonResult buscarProductojq(string term)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            var finded = db.Products.Where(p => p.CompanyId == usuario.CompanyId && p.Nombreproducto.StartsWith(term)).Select(p => p.Nombreproducto).ToList();

            return Json(finded, JsonRequestBehavior.AllowGet);
        }


        // GET: Products
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = db.Products.Where(p => p.CompanyId == usuario.CompanyId).Include(p => p.Category).Include(p => p.Supplier).Include(p => p.UnitMeasure).OrderByDescending(p => p.ProductId);
            return View(products.ToPagedList((int)page, 15));
        }


        [HttpPost]
        public ActionResult Index(string term, int? page = null)
        {
            page = (page ?? 1);
            bool longsi;
            long barcodigo;
            string namepro;

            longsi = long.TryParse(term, out barcodigo);

            if (longsi)
            {
                if (barcodigo > 0)
                {
                    var usuarioi = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
                    var producto = db.Products.Where(p => p.CompanyId == usuarioi.CompanyId  && p.Codigobarra == barcodigo ).OrderBy(p => p.ProductId);
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
                namepro = term;
                var usuarioi = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
                var producto = db.Products.Where(p =>  p.CompanyId == usuarioi.CompanyId && p.Nombreproducto.StartsWith(namepro)).OrderBy(p => p.ProductId);
                //TODO: agregar aqui los usuarios a los que p
                return View(producto.ToPagedList((int)page, 10));
            }
        }

        public ActionResult comprar(int? id)
        {

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            //var bodega = db.Warehouses.Where(b => b.CompanyId == usuario.CompanyId).FirstOrDefault();

            //var inventario = db.Inventories.Where(i => i.ProductId == id && i.WarehouseId == bodega.WarehouseId).FirstOrDefault();

            /* if (inventario == null)
             {
                 var inventarioget = new Purchase
                 {
                     WarehouseId = bodega.WarehouseId,
                     ProductId = id,
                     Existencia = 0,
                     UltimoAdd = 0,
                     FechaCreada = DateTime.Today,
                     FechaActualizada = DateTime.Today,
                     UserId = usuario.UserId
                 };
                 //ViewBag.inventario = inventarioget;
                 return PartialView(inventarioget);
             }

             //ViewBag.inventario = inventario;*/

            return View();
        }

        [HttpPost]
        public ActionResult comprar(int idbodega, int idinventario, int idproducto, int newcant)
        {
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            

            //if (newcant <= 0)
            //{
            //    return RedirectToAction("Details/" + idproducto, "Products");
            //}

            //var inventario = db.Inventories.Find(idinventario);
            //if (inventario == null)
            //{
            //   var  inventarionew = new Purchase {
            //            WarehouseId = idbodega,
            //            ProductId = idproducto,
            //            Existencia = newcant,
            //            UltimoAdd = newcant,
            //            FechaCreada = DateTime.Today,
            //            FechaActualizada = DateTime.Today,
            //            UserId = usuario.UserId
            //    };

            //    db.Inventories.Add(inventarionew);
            //    var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            //    if (respuesta.Succeeded)
            //    {
            //        return RedirectToAction("Details/" + idproducto, "Products");
            //    }
            //    ModelState.AddModelError(string.Empty, respuesta.Message);
            //}

           
           

            //var catidad = (from i in db.Inventories
            //               where i.ProductId == idproducto
            //               select i.Existencia).FirstOrDefault();

            //var oldexist = int.Parse(catidad.ToString());
            //var newexist = oldexist + newcant;

            //inventario.Existencia = newexist;
            //inventario.UltimoAdd = newcant;
            //inventario.FechaActualizada = DateTime.Today;
            //inventario.UserId = usuario.UserId;

            //var respueta = ChangeValidationHelperDb.ChangeDb(db);
            //if (respueta.Succeeded)
            //{
            //    ViewBag.inventario = inventario;

            //    return RedirectToAction("Details/" + idproducto, "Products");
            //}
            //ModelState.AddModelError(string.Empty, respueta.Message);

            return View();
            
        }

        [HttpPost]
        public ActionResult fileCSV(HttpPostedFileBase file)
        {
            string filePath = string.Empty;

            if (file != null)
            {
                if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx"))
                {
                    TempData["mensaje"] = "Debe seleccionar un archivo con extencion (.xls) o (xlsx).";
                    return RedirectToAction("index");
                }

                string path = Server.MapPath("~/excel/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                var fileName = filePath;
                //DateTime.Now.ToString("yyyyMMddHHmm.") + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last()
                file.SaveAs(fileName);

                //string csv = System.IO.File.ReadAllText(filePath);

                var records = new Product();

                using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/excel/"), fileName), FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            records = new Product
                            {
                                Codigobarra = Convert.ToInt64(reader.GetValue(0)),
                                Nombreproducto = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                Preciocompra = Convert.ToDecimal(reader.GetValue(3)),
                                Precioventa = Convert.ToDecimal(reader.GetValue(4)),
                                PrecioCompraNew = 0,
                                Uso = reader.GetString(5),
                                Ubicacion = reader.GetString(6),
                                PrincipioActivo = reader.GetString(7),
                                Porcentaje = ((Convert.ToDecimal(reader.GetValue(3)) / Convert.ToDecimal(reader.GetValue(4))) - 1) / -1,
                                UnitMeasureId = Convert.ToInt32(reader.GetValue(8)),
                                CategoryId = Convert.ToInt32(reader.GetValue(9)), //modificar id
                                SupplierId = Convert.ToInt32(reader.GetValue(10)),
                                CompanyId = 1
                         
                            };

                            db.Products.Add(records);
                        }
                    }
                }

                #region csv
               /* foreach (string row in csv.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var productoExcel = new Product
                        {
                            Codigobarra = Convert.ToInt64(row.Split(';')[0]),
                            Nombreproducto = row.Split(';')[1],
                            Descripcion = row.Split(';')[2],
                            Preciocompra = Convert.ToDecimal(row.Split(';')[3]),
                            Precioventa = Convert.ToDecimal(row.Split(';')[4]),
                            PrecioCompraNew = Convert.ToDecimal(row.Split(';')[5]),
                            Uso = row.Split(';')[6],
                            Ubicacion = row.Split(';')[7],
                            PrincipioActivo = row.Split(';')[8],
                            Porcentaje = 0,
                            UnitMeasureId = 1,
                            CategoryId = 1, //modificar id
                            SupplierId = 6,
                            CompanyId = 1
                        };

                        db.Products.Add(productoExcel);
                    }
                }*/
                #endregion


                //db.SaveChanges();
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");

                }


                TempData["mensaje"] = respuesta.Message;
                return RedirectToAction("Create", "Products");
            }

            TempData["mensaje"] = "El archivo seleccionado parece estar vacio.";
            return RedirectToAction("Create", "Products");
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

            ViewBag.UnitMeasureId = new SelectList(cboAll.getUnidad(), "UnitMeasureId", "Tipo");
            ViewBag.CategoryId = new SelectList(cboAll.getCategory(), "CategoryId", "Descripcion");
            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
            ViewBag.SupplierId = new SelectList(cboAll.getProveedor(), "SupplierId", "Nombre");

            //db.Products.Where(p => p.CompanyId == usuario.CompanyId).FirstOrDefault();

            var producto = new Product { CompanyId = usuario.CompanyId};

            return View(producto);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                
                product.Porcentaje = ((product.Preciocompra / product.Precioventa) - 1) / -1;
               
                db.Products.Add(product);
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "El producto con este Codigo de Barra ya Existe.");
            }

            ViewBag.UnitMeasureId = new SelectList(cboAll.getUnidad(), "UnitMeasureId", "Tipo", product.UnitMeasureId);
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

            ViewBag.UnitMeasureId = new SelectList(cboAll.getUnidad(), "UnitMeasureId", "Tipo", product.UnitMeasureId);
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
                product.Porcentaje = ((product.Preciocompra / product.Precioventa) - 1) / -1;
                db.Entry(product).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            ViewBag.UnitMeasureId = new SelectList(cboAll.getUnidad(), "UnitMeasureId", "Tipo", product.UnitMeasureId);
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
