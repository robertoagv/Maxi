using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(25, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Proveedor")]
        public string Nombre { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Debe ingresar un dato numerico.")]
        [DisplayFormat(DataFormatString = "{0:##-##-##}", ApplyFormatInEditMode = false)]
        public int Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Correo Invalido")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres.")]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        //Relaciones
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Purchase> Purcharses { get; set; }
    
    }
}