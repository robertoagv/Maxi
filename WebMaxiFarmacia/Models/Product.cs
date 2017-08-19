using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Index("product_codbarsuc_index", 2, IsUnique = true)]
        [Display(Name = "Codigo Barra")]
        public long Codigobarra { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Producto")]
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

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar a {0} entre {1} y {2}")]
        [Display(Name = "Compra Nueva")]
        public decimal PrecioCompraNew { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Uso")]
        public string Uso { get; set; }

        
        [MaxLength(250, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Ubicacion")]
        public string Ubicacion { get; set; }

        
        [MaxLength(250, ErrorMessage = "El campo {0} debe contener maximo {1} caracteres)")]
        [Display(Name = "Principio Activo")]
        public string PrincipioActivo { get; set; }

        [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = false)]
        public decimal Porcentaje { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Unidad de Medida")]
        public int UnitMeasureId  { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Display(Name = "Proveedor")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe Seleccionar un {0}")]
        [Index("product_codbarsuc_index", 1, IsUnique = true)]
        [Display(Name = "Sucursal")]
        public int CompanyId { get; set; }


        //propiedades solo de lectura, para el inventario.
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public int? Existencia { get { return Inventories.Sum(i => i.Existencia); } }




        //Relaciones
        public virtual UnitMeasure UnitMeasure { get; set; }
        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Company Company { get; set; }
      

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }

        public virtual ICollection<SaleDatilsTmp>  SaleDatailTmps { get; set; }


    }
}