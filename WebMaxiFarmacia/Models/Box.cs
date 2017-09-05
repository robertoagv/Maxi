using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Box
    {
        [Key]
        public int BoxId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal valor { get; set; }

        public DateTime Fecha { get; set; }

        public string usuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }

        //ralaciones
        public virtual Company Company { get; set; }
    } 
}