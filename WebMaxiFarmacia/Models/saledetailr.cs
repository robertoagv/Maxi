﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMaxiFarmacia.Models
{
    public class saledetailr
    {
        public int saleId { get; set; }
        public string cliente { get; set; }
        public DateTime fecha { get; set; }
        public long Codigo { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int cantidad { get; set; }
        public decimal valortotal { get; set; }
        public string  usuario { get; set; }
          
    } 
}