using Microsoft.AspNetCore.Mvc;


namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
  
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = new ML.Result();
            usuario.Rol = new ML.Rol();
            
            ML.Result resultRol = BL.Rol.GetAll();
            result = BL.Usuario.GetAll(usuario);

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Alumno/GetAll");
                    
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItem);
                        }

                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }

            if (result.Correct)
            {
                usuario.Rol.Rols = resultRol.Objects;
                usuario.Usuarios = result.Objects;
                return View(usuario);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al realizar la consulta de Usuario";
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            ML.Result resultRol = BL.Rol.GetAll();
            result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                usuario.Rol.Rols = resultRol.Objects;

                return View(usuario);
            }
            else
            {
                ViewBag.Mensaje = "Ocurrio un error al consultar los usuarios";
                return View();
            }
        }

        [HttpGet]//Muestra las vistas
        public ActionResult Form(int? Matricula)
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPaises = BL.Pais.GetAll();
            ML.Usuario usuario = new ML.Usuario();


            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();


            if (Matricula == null)
            {
                usuario.Rol.Rols = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;
                return View(usuario);
            }
            else
            {
                //GetbyId
                ML.Result result = BL.Usuario.GetById(Matricula.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                    usuario.Rol.Rols = resultRol.Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;

                    ML.Result resultEstados = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar el Usuario";
                }
                return View(usuario);
            }

        }
        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            IFormFile imagen = Request.Form.Files["IFImage"];

            if (imagen != null)
            {
                //llamar al metodo que convierte a bytes la imagen
                byte[] ImagenBytes = ConvertToBytes(imagen);
                //convierto a base 64 la imagen y la guardo en la propiedad de imagen en el objeto alumno
                usuario.Imagen = Convert.ToBase64String(ImagenBytes);
            }
            if (!ModelState.IsValid)
            {
                usuario.Rol = new ML.Rol();
                ML.Result resultRol = BL.Rol.GetAll();
                ML.Result resultPaises = BL.Pais.GetAll();

                usuario.Rol.Rols = resultRol.Objects;
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;


                return View(usuario);
            }
            else
            {
                ML.Result result = new ML.Result();

                if (usuario.Matricula == 0)
                {
                    //Add
                    result = BL.Usuario.Add(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = result.Message;
                    }
                    else
                    {
                        ViewBag.Message = "Error: " + result.Message;
                    }
                }
                else
                {
                    //update
                    result = BL.Usuario.Update(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Massage = "Se actualizo correctamente el Usuario. " + result.Message;

                    }
                    else
                    {
                        ViewBag.Message = "Error " + result.Message;
                    }
                }
                return View("Modal");
            }
        
        }
        //DELETE
        [HttpGet]
        public ActionResult Delete(int Matricula)
        {
            ML.Result result = new ML.Result();

            if (result.Correct)
            {
                ViewBag.Mensaje = "Se ha elimnado el registro";
          
            }
            else
            {
                ViewBag.Mensaje = "No se ha elimnado el registro" + result.Message;
            
            }
            return View("Modal");
        }
        public JsonResult GetEstado(int IdPais)
        {
            var result = BL.Estado.GetByIdPais(IdPais);

            return Json(result.Objects);
        }

        public JsonResult GetMunicipio(int IdEstado)
        {
            var result = BL.Municipio.GetByIdEstado(IdEstado);

            return Json(result.Objects);
        }
        public JsonResult GetColonia(int IdMunicipio)
        {
            var result = BL.Colonia.GetByIdMunicipio(IdMunicipio);

            return Json(result.Objects);
        }
        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public JsonResult CambiarStatus(int matricula, bool status)
        {
            ML.Result result = BL.Usuario.ChangeStatus(matricula, status);

            return Json(result);
        }

    }
}
