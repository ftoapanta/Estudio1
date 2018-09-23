using System;
using System.Collections.Generic;
using System.Text;

namespace Estudio1.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public string Moneda { get; set; }
        public decimal Tasa { get; set; }
        public string Nombre { get; set; }
    }
}
