using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlToExcel.Models
{
    public class NodeElement
    {
        public string ElementName { get; set; } = null!;
        public List<XAttribute> Attributes { get; set; } = [];
    }
}
