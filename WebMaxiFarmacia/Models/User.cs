using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Codigo Barra")]
        public long Codigobarra { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombreproducto { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar a {0} entre {1} y {2}")]
        [Display(Name = "Precio Compra")]
        public decimal Preciocompra { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar a {0} entre {1} y {2}")]
        [Display(Name = "Precio Venta")]
        public decimal Precioventa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Proveedor")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }
    }
}