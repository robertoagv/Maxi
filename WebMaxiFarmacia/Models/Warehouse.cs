using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public int Telefono { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar una {0}")]
        [Index("bodega_conpannyid_index", IsUnique =true)]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }


        //Relaciones
        public virtual Company  Company { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
       

    }
}