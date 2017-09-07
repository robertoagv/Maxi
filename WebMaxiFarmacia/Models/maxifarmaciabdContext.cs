using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class maxifarmaciabdContext : DbContext
    {
        public maxifarmaciabdContext() : base("DefaultConnection")
        {
                
        }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Warehouse> Warehouses { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<SaleDatilsTmp> SaleDetailTmps { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)

        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.UnitMeasure> UnitMeasures { get; set; }

        public System.Data.Entity.DbSet<WebMaxiFarmacia.Models.Box> Boxes { get; set; }
    }

    
}