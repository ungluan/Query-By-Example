using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Model
{
    public class Row
    {
        public List<String> Tables { get; set; }
        public List<String> Fields { get; set; }
        public List<String> Selects { get; set; }
        public String Where { get; set; }
        public String Having { get; set; }
        public List<String> Sort { get; set; }
        public String Alias { get; set; }

    }
}