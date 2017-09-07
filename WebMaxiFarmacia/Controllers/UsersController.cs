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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebMaxiFarmacia.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UsersController : Controller
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboAll = new cboAll();
        [AllowAnonymous]
        public ActionResult enviarMail()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> enviarMail(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByEmail(email);
            var user = db.Users.Where(tp => tp.NombreUser == email).FirstOrDefault();

            if (userASP != null && user != null)
            {
                await UserHelper.PasswordRecovery(email);
                return RedirectToAction("Login", "Account");
            }

            ViewBag.mensaje = "No Existe el Usuario o el Correo es Invalido.";
            return View();

           
        }


        // GET: Users
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = db.Users.Where(ua => ua.CompanyId == usuario.CompanyId && ua.estado != 1).Include(u => u.Company).Include(u => u.Employee).OrderByDescending(u => u.NombreUser);

            return View(users.ToPagedList((int)page, 10));
        }
        [HttpPost]
        public ActionResult Index(string termino, int? page = null)
        {
            page = (page ?? 1);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var users = db.Users.Where(ua => ua.CompanyId == usuario.CompanyId && ua.NombreUser == termino && ua.estado != 1).Include(u => u.Company).Include(u => u.Employee).OrderByDescending(u => u.NombreUser);

            return View(users.ToPagedList((int)page, 10));
        }


        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        

        // GET: Users/Create
        public ActionResult Create()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var companyid = db.Companies.OrderByDescending(c => c.CompanyId).FirstOrDefault();
                var empleado = db.Employees.Where(e => e.CompanyId == companyid.CompanyId).ToList();
                empleado.Add(new Employee()
                {
                    EmployeeId = 0,
                    Nombreemp = "[Seleccione un Empleado]"
                });

                //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
                ViewBag.EmployeeId = new SelectList(empleado.OrderBy(e => e.Nombreemp).ToList(), "EmployeeId", "Nombreemp");
                var user = new User { CompanyId = companyid.CompanyId };
                return View(user);
            }


            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            var empleadoCbo = db.Employees.Where(e => e.CompanyId == usuario.CompanyId).ToList();

            empleadoCbo.Add(new Employee()
                {
                    EmployeeId = 0,
                    Nombreemp = "[Seleccione un Empleado]"
                });

                 //empleadoCbo.Where(e => e.CompanyId == usuario.CompanyId).OrderBy(e => e.Nombreemp).ToList();

            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
            ViewBag.EmployeeId = new SelectList(empleadoCbo.OrderBy(e => e.Nombreemp).ToList(), "EmployeeId", "Nombreemp");

            var usuarioget = new User { CompanyId = usuario.CompanyId };

            return View(usuarioget);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, string role)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);

                if (respuesta.Succeeded)
                {
                    UserHelper.CreateUserASP(user.NombreUser, role);
                    return RedirectToAction("Index"); 
                }

                ModelState.AddModelError(string.Empty, "Este usuario ya Existe.");
            }
            
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            var empleadoCbo = db.Employees.Where(e => e.CompanyId == usuario.CompanyId).ToList();
            empleadoCbo.Add(new Employee()
            {
                EmployeeId = 0,
                Nombreemp = "[Seleccione un Empleado]"
            });

            

            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            ViewBag.EmployeeId = new SelectList(empleadoCbo.OrderBy(e => e.Nombreemp).ToList(), "EmployeeId", "Nombreemp", user.EmployeeId);
            return View(user);
        }

        public ActionResult EliminarEditar(int? id)
        {
            string newusuario;
            if (id != null)
            {
                var dbBuscarEmailOld = new maxifarmaciabdContext();
                var oldUser = dbBuscarEmailOld.Users.Find(id);
                if (oldUser != null)
                {
                    newusuario = "usuario" + oldUser.UserId + "@nulo.com";
                    UserHelper.UpdateUser(oldUser.NombreUser, newusuario);
                }
                dbBuscarEmailOld.Dispose();

                oldUser.NombreUser = "usuario" + oldUser.UserId + "@nulo.com";
                oldUser.estado = 1;
                db.Entry(oldUser).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);

                if (respuesta.Succeeded)
                {
                    
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            TempData["mensaje"] = "No se pudo Eliminar el Usuario";
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            var empleadoCbo = db.Employees.Where(e => e.CompanyId == usuario.CompanyId).ToList();
            empleadoCbo.Add(new Employee()
            {
                EmployeeId = 0,
                Nombreemp = "[Seleccione un Empleado]"
            });


            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            //ViewBag.EmployeeId = new SelectList(empleadoCbo.OrderBy(e => e.Nombreemp).ToList(), "EmployeeId", "Nombreemp", user.EmployeeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dbBuscarEmailOld = new maxifarmaciabdContext();
                var oldUser = dbBuscarEmailOld.Users.Find(user.UserId);
                if (oldUser.NombreUser != user.NombreUser)
                {
                    UserHelper.UpdateUser(oldUser.NombreUser, user.NombreUser);
                }
                dbBuscarEmailOld.Dispose();

                db.Entry(user).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);

                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }
                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();

            var empleadoCbo = db.Employees.Where(e => e.CompanyId == usuario.CompanyId).ToList();
            empleadoCbo.Add(new Employee()
            {
                EmployeeId = 0,
                Nombreemp = "[Seleccione un Empleado]"
            });

            //ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            //ViewBag.EmployeeId = new SelectList(empleadoCbo.OrderBy(e => e.Nombreemp).ToList(), "EmployeeId", "Nombreemp", user.EmployeeId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                UserHelper.DeleteUser(user.NombreUser);//lo borra o no.
                return RedirectToAction("Index"); 
            }

            ModelState.AddModelError(string.Empty, respuesta.Message);
            return View(user);
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
