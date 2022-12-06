using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;

namespace PL.Controllers
{
    public class VentaProductoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = new ML.Result(); //instanceamos la clase result 
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();

            ML.Result resultDepartamento = BL.Departamento.GetAll();
            result = BL.Producto.GetAll(producto); //llamamos el metodo

            if (result.Correct) //validacion 
            {
                // ML.Producto producto = new ML.Producto();
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Productos = result.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Massage = "Ocurrio un error al consultar los productos. ";
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetAll(ML.Producto producto)
        {

            ML.Result result = new ML.Result(); //instanceamos la clase result 
                                                // producto.Departamento = new ML.Departamento();

            ML.Result resultDepartamento = BL.Departamento.GetAll();
            result = BL.Producto.GetAll(producto); //llamamos el metodo

            if (result.Correct) //validacion 
            {
                // ML.Usuario usuario = new ML.Usuario();
                producto.Productos = result.Objects;
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Massage = "Ocurrio un error al consultar los usuarios. ";
                return View();
            }
        }
        [HttpGet]
        public ActionResult CartPost(ML.Producto producto)
        {
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentaProductos = new List<object>();

            if (HttpContext.Session.GetString("Producto") == null)
            {
                producto.Stock = producto.Stock = 1;
                ventaProducto.VentaProductos.Add(producto);
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                var session = HttpContext.Session.GetString("Producto");
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentaProductos.Add(objProducto);
                }

                foreach (ML.Producto venta in ventaProducto.VentaProductos.ToList())
                {
                    if (producto.IdProducto == venta.IdProducto)
                    {
                        venta.Stock = venta.Stock + 1;   //True                
                    }
                    else
                    {
                        producto.Stock = producto.Stock = 1; //False
                        ventaProducto.VentaProductos.Add(producto);
                    }
                }
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
            }
            if (HttpContext.Session.GetString("Producto") != null)
            {
                ViewBag.Message = "Se ha agregado el producto a tu carrito!";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "No se pudo agregar tu producto ):";
                return PartialView("Modal");
            }

        }
            [HttpGet]
        public ActionResult ResumenCompra(ML.VentaProducto ventaProducto)
        {
            if (HttpContext.Session.GetString("Producto") == null)
            {
                return View();
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));
                ventaProducto.VentaProductos = new List<object>();
                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentaProductos.Add(objProducto);
                }

            }

            return View(ventaProducto);
        }

    }
}
