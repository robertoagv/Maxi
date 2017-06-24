﻿using System;
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
        public string Nombrecte { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fechavta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Usuario")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }

        //Relaciones

        public virtual User  Users { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }


    }
}