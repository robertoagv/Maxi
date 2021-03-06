﻿using System;
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
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class EmployeesController : Controller
    {
        private maxifarmaciabdContext db = new maxifarmaciabdContext();
        private cboAll cboEmp = new cboAll();

        // GET: Employees
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var employees = db.Employees.Where(e => e.CompanyId == usuario.CompanyId).Include(e => e.Company).OrderByDescending(e => e.EmployeeId);

            return View(employees.ToPagedList((int)page, 5));
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

            var employees = db.Employees.Where(e => e.CompanyId == usuario.CompanyId && e.Nombreemp == termino).Include(e => e.Company).OrderByDescending(e => e.EmployeeId);

            return View(employees.ToPagedList((int)page, 10));
        }


        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            

            if (User.IsInRole("SuperAdmin"))
            {
                ViewBag.CompanyId = new SelectList(cboEmp.getSucursal(), "CompanyId", "nombresuc");
                var empleadosuper = new Employee { Telefonoemp = 00, Direccionemp = "--------" };
                return View(empleadosuper);
            }

            var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
            
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var empleado = new Employee { CompanyId = usuario.CompanyId };

            //ViewBag.CompanyId = new SelectList(cboEmp.getSucursal(), "CompanyId", "nombresuc");

            return View(empleado);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                  return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            ViewBag.CompanyId = new SelectList(cboEmp.getSucursal(), "CompanyId", "nombresuc", employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CompanyId = new SelectList(cboEmp.getSucursal(), "CompanyId", "nombresuc", employee.CompanyId);
            return View(employee);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                var respuesta = ChangeValidationHelperDb.ChangeDb(db);
                if (respuesta.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Message);
            }

            //ViewBag.CompanyId = new SelectList(cboEmp.getSucursal(), "CompanyId", "nombresuc", employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            var respuesta = ChangeValidationHelperDb.ChangeDb(db);
            if (respuesta.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, respuesta.Message);

            return View(employee);
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
