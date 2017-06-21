using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class NewSaleView
    {

        [MaxLength(100, ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombrecte { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fechavta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Usuario")]
        public int UserId { get; set; }


        public List<SaleDatilsTmp> Detalles { get; set; }

        
        public int TotalCantidad { get { return Detalles == null ? 0 : Detalles.Sum(d => d.Cantidad); } }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalValue { get { return Detalles == null ? 0 : Detalles.Sum(d => d.Valor); } }



    }
}