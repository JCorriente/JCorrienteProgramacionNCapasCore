using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class VentaProducto
    {
        public int IdVentaProducto { get; set; }

        public string Venta { get; set; }
        public int Stock { get; set; }
        public ML.Producto Producto { get; set; }
        public List<object> VentaProductos{ get; set; }
  
    }
}
