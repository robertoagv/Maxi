using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.classHelper
{
    public class ReportInventory
    {
        public Int64 CodigoBarra { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioNuevo { get; set; }
        public decimal Porcentaje { get; set; }
        public string UnidadMedida { get; set; }
        public int existencia { get; set; }
        public decimal TotalCosto { get; set; }
        public decimal TotalVenta { get; set; }
        public int Entrante { get; set; }
        public int Saliente { get; set; }
        public string FechaVencimiento { get; set; }

    }
}