using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class SaleDetail
    {
        [Key]
        public int SaleDetailId { get; set; }
    
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Producto")]
        public string Descriptionpro { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        //[DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "En este campo {0} ingrese valores de {1} a {2}")]
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public int Cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorU{ get { return Price * (decimal)Cantidad; } }

        

        //relaciones

        public virtual Product Product { get; set; }
        public virtual Sale Sale { get; set; }

    }
}