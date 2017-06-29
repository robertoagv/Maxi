using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio.")]
        [MaxLength(256, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Usuario")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo {0} no es un Correo Valido.")]
        [Index("user_nameuser_index", IsUnique = true)]
        public string NombreUser { get; set; }

        [Display(Name = "Empleado")]
        public int EmployeeId { get; set; }

        [Display(Name = "Sucursal")]
       
        public int CompanyId { get; set; }


        //Relaciones.

        public virtual Employee Employee { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }




    }
}