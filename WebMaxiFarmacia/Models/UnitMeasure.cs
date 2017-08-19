using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class UnitMeasure
    {
        [Key]
        public int UnitMeasureId { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Tipo { get; set; }

    

        public virtual ICollection<Product> Products { get; set; }
    
    }
}