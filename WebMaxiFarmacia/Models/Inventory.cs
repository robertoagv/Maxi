using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Inventory
    {
        [Key]
        public int inventoryId { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int? WarehouseId { get; set; }

        // [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int? ProductId { get; set; }

        public double Existencia { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
    }
}