using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDb.AdoNet
{
    public class Column : Attribute
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int Order { get; set; }
    }
}
