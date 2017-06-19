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
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboAll = new cboAll();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Company).Include(u => u.Employee);
            return View(users.ToList());
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
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc");
            ViewBag.EmployeeId = new SelectList(cboAll.getEmpleado(), "EmployeeId", "Nombreemp");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();

                UserHelper.CreateUserASP(user.NombreUser, "User");

                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            ViewBag.EmployeeId = new SelectList(cboAll.getEmpleado(), "EmployeeId", "Nombreemp", user.EmployeeId);
            return View(user);
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
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            ViewBag.EmployeeId = new SelectList(cboAll.getEmpleado(), "EmployeeId", "Nombreemp", user.EmployeeId);
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
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(cboAll.getSucursal(), "CompanyId", "nombresuc", user.CompanyId);
            ViewBag.EmployeeId = new SelectList(cboAll.getEmpleado(), "EmployeeId", "Nombreemp", user.EmployeeId);
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
            db.SaveChanges();
            UserHelper.DeleteUser(user.NombreUser);//lo borra o no.
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
