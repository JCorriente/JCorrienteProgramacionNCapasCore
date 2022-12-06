using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
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

        [HttpGet]//muestra las vistas
        public ActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();
            producto.Proveedor = new ML.Proveedor();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();


            ML.Result resultDepartamento = BL.Departamento.GetAll();
            // ML.Result resultPaises = BL.Pais.GetAll();

            if (IdProducto == null)
            {
                //usuario.Rol.Roles = resultRol.Objects;
                //usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;
                return View(producto);
            }
            else
            {
                //GetbyId
                ML.Result result = BL.Producto.GetById(IdProducto.Value);

                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;
                    producto.Departamento.Departamentos = resultDepartamento.Objects;
                    //.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;

                    //ML.Result resultEstados = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    //usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar el Usuario";
                }
                return View(producto);
            }
          
        }

        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            IFormFile imagen = Request.Form.Files["IFImage"];

            if (imagen != null)
            {
                //llamar al metodo que convierte a bytes la imagen
                byte[] ImagenBytes = ConvertToBytes(imagen);
                //convierto a base 64 la imagen y la guardo en la propiedad de imagen en el objeto alumno
                producto.Imagen = Convert.ToBase64String(ImagenBytes);
            }

            else
            { 
                ML.Result result = new ML.Result();

                if (producto.IdProducto == 0)
                {
                    //ADD
                    result = BL.Producto.Add(producto);
                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error" + result.Message;
                    }
                }
                else
                {
                    //Update
                    result = BL.Producto.Update(producto);

                    if (result.Correct)
                    {
                        ViewBag.Massage = "Se actualizo correctamente el producto. " + result.Message;
                        //return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Massage = "Error: " + result.Message;
                        //return PartialView("Modal");
                    }
                }
                
            }
            return View("Modal");
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

    }
}
