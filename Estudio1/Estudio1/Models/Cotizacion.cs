using System;
using System.Collections.Generic;
using System.Text;

namespace Estudio1.Models
{
    public class Cotizacion
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }

    }
    

    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
    }

    public class RootObject
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }
}
