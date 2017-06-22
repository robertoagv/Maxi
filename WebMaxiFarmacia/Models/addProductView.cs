using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class addProductView
    {
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Producto")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public long Codigobarra { get; set; }

        public string Nombreproducto  { get; set; }
        public decimal Precioventa { get; set; }
        public double? Existencia { get; set; }
    
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, int.MaxValue, ErrorMessage = "En el campo {0} debes ingresar numeros mayores a {1}.")]
        public int Cantidad { get; set; }
    }
}