using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlToExcel.Models
{
    public class Concepto
    {
        public bool SaveData { get; set; } = true;
        public string Cantidad { get; set; } = null!;
        public string ClaveProdServ { get; set; } = null!;
        public string ClaveUnidad { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Importe { get; set; } = null!;
        public string NoIdentificacion { get; set; } = null!;
        public string ObjetoImp { get; set; } = null!;
        public string Unidad { get; set; } = null!;
        public string ValorUnitario { get; set; } = null!;

        public Concepto(XAttribute[] Elements)
        {
            Cantidad = Elements[0].Value;
            ClaveProdServ = Elements[1].Value;
            ClaveUnidad = Elements[2].Value;
            Descripcion = Elements[3].Value;
            Importe = Elements[4].Value;
            NoIdentificacion = Elements[5].Value;
            ObjetoImp = Elements[6].Value;
            Unidad = Elements[7].Value;
            ValorUnitario = Elements[8].Value;
        }
    }
}
