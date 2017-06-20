using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMaxiFarmacia.Models;
using System.Web.Mvc;

namespace WebMaxiFarmacia.classHelper
{
    public class cboAll : Controller
    {
        
      
        public List<Supplier> getProveedor()
        {
            using(var db = new maxifarmaciabdContext())
            {
               var  proveedorCbo = db.Suppliers.ToList();
                proveedorCbo.Add(new Supplier()
                {
                    SupplierId = 0,
                    Nombre = "[Seleccione un Proveedor...]"
                });

                return proveedorCbo.OrderBy(p => p.Nombre).ToList();
            } 
        }

        public List<Category> getCategory()
        {
            using (var db = new maxifarmaciabdContext())
            {
                var catergoriaCbo = db.Categories.ToList();

                catergoriaCbo.Add(new Category() {
                    CategoryId = 0,
                    Descripcion = "[Seleccione una Categoria...]"
                });

                return catergoriaCbo.OrderBy(c => c.Descripcion).ToList();
            }
        }

        public List<Company> getSucursal()
        {
            using (var db = new maxifarmaciabdContext())
            {
                //var usuario = db.Users.Where(u => u.NombreUser == User.Identity.Name).FirstOrDefault();
                //var sucursalCbo = db.Companies.Where(s => s.CompanyId == usuario.CompanyId).ToList();

                var sucursalCbo = db.Companies.ToList();
                sucursalCbo.Add(new Company()
                {
                    CompanyId = 0,
                    nombresuc = "[Seleccione una Sucursal...]"
                });

                return sucursalCbo.OrderBy(s => s.nombresuc).ToList();
            }
        }

        

    }
}