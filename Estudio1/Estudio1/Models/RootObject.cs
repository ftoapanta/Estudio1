using System;
using System.Collections.Generic;
using System.Text;

namespace Estudio1.Models
{

    public class Rootobject
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<rate> rates { get; set; }
        public Rootobject()
        {

        }
    }
}
