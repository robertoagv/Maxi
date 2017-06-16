using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Category
    {
        //Campos de la tabla
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
        [Display(Name = "Categoria")]
        public string Descripcion { get; set; }

        //Relaciones
        public virtual ICollection<Product> Products { get; set; }

    }
}