using System;
using System.Collections.Generic;
using System.Text;

namespace Estudio1.Models
{
    public class Cotizacion
    {
        public int CotizacionId { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
      
    }
}
