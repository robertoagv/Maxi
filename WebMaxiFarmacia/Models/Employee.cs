using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Nombres")]
        public string Nombreemp { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Apellidos")]
        public string Apellidoemp { get; set; }


        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo {0} debe ser Numerico.")]
        [DisplayFormat(DataFormatString = "{0:##-##-##}", ApplyFormatInEditMode = false)]
        [Display(Name = "Telefono")]
        public int Telefonoemp { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(250, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Direccion")]
        public string Direccionemp { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }

        //no mapeados.
        


        //Relaciones
        public virtual Company Company { get; set; }
        public virtual ICollection<User> Users { get; set; }
        

    }
}