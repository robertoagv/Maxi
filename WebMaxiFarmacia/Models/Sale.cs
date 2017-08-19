using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Cliente")]
        public string Nombrecte { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Venta")]
        public DateTime Fechavta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Usuario")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }


        public List<SaleDetail> Detalles { get; set; }
        public List<Sale> total { get; set; }
        public List<Sale> totalc { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Total Cantidad")]
        public int TotalCantidad { get { return Detalles == null ? 0 : Detalles.Sum(d => d.Cantidad); } }

        [Display(Name = "Total Cantidad")]
        public int TotalCantidadT { get { return Detalles == null ? 0 : totalc.Sum(d => d.TotalCantidad); } }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Total Valor")]
        public decimal TotalValue { get { return Detalles == null ? 0 : Detalles.Sum(d => d.ValorU); } }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Total Valor")]
        public decimal TotalValueT { get { return Detalles == null ? 0 : total.Sum(d => d.TotalValue); } }

        //Relaciones

        public virtual User  Users { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }


    }
}