using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Inventory
    {
        [Key]
        public int inventoryId { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Bodega")]
        public int WarehouseId { get; set; }

        // [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Producto")]
        public int? ProductId { get; set; }

        public int Existencia { get; set; }
        [Display(Name = "Ultima Entrada")]
        public int UltimoAdd { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de Creacion")]
        public DateTime FechaCreada { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de Actalizacion")]
        public DateTime FechaActualizada { get; set; }

        [Display(Name = "Usuario")]
        public int? UserId { get; set; }

        [NotMapped]
        public string nombrep { get; set; }
        [NotMapped]
        public long codigobarra { get; set; }
        [NotMapped]
        public string nombreu { get; set; }
         
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }

    } 
}