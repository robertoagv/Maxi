using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Sucursal")]
        public string nombresuc { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} debe ser Numerico.")]
        [DisplayFormat(DataFormatString  = "{0:##-##-##}", ApplyFormatInEditMode = false)]
        [Display(Name = "Telefono")]
        public int telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Direccion")]
        public string direccion { get; set; }

        [EmailAddress(ErrorMessage = "Debe ingresar un correo valido")]
        [Display(Name = "Correo")]
        public string email { get; set; }

        //Relaciones
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection <User> Users { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
         
    }
}