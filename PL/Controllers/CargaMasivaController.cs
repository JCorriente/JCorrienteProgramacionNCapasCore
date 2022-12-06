using Microsoft.AspNetCore.Mvc;
using ML;
using System.IO;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CargaMasivaController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult CargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult CargaTXT()
        {
            IFormFile fileTXT = Request.Form.Files["archivoTXT"];

            if (fileTXT != null)
            {
                StreamReader Textfile = new StreamReader(fileTXT.OpenReadStream());

                string line;
                line = Textfile.ReadLine();

                ML.Result resultErrores = new ML.Result();
                resultErrores.Objects = new List<object>();

                while ((line = Textfile.ReadLine()) != null)
                {
                    string[] lines = line.Split('|');

                    ML.Usuario usuario = new ML.Usuario();

                    usuario.Nombre = lines[0];
                    usuario.ApellidoPaterno = lines[1];
                    usuario.ApellidoMaterno = lines[2];
                    usuario.FechaNacimiento = lines[3];
                    usuario.Genero = lines[4];
                    usuario.Email = lines[5];
                    usuario.Password = lines[6];
                    usuario.Telefono = lines[7];
                    usuario.Celular = lines[8];
                    usuario.CURP = lines[9];
                    usuario.UserName = lines[10];

                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = byte.Parse(lines[11]);
                    usuario.Imagen = null;

                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Calle = lines[12];
                    usuario.Direccion.NumeroExterior = lines[13];
                    usuario.Direccion.NumeroInterior = lines[14];

                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = int.Parse(lines[15]);

                    ML.Result result = BL.Usuario.Add(usuario);

                    if (!result.Correct)
                    {
                        resultErrores.Objects.Add(
                            "No se inserto el Nombre correctamente" + usuario.Nombre +
                            "No se inserto el Apellido Paterno correctamente" + usuario.ApellidoPaterno +
                            "No se inserto el Apellido Materno correctamente" + usuario.ApellidoMaterno +
                            "No se inserto la Fecha de nacimiento correctamente" + usuario.FechaNacimiento +
                            "No se inserto el Genero correctamente" + usuario.Genero +
                            "No se inserto el Email de nacimiento correctamente" + usuario.Email +
                            "No se inserto el Password correctamente" + usuario.Password +
                            "No se inserto el Telefono de nacimiento correctamente" + usuario.Telefono +
                            "No se inserto el Celular de nacimiento correctamente" + usuario.Celular +
                            "No se inserto el CURP de nacimiento correctamente" + usuario.CURP +
                            "No se inserto la Imagen correctamente" + usuario.Imagen +
                            "No se inserto el IdRol correctamente" + usuario.Rol.IdRol +
                            "No se inserto la Calle de nacimiento correctamente" + usuario.Direccion.Calle +
                            "No se inserto el NumeroExterior de nacimiento correctamente" + usuario.Direccion.NumeroExterior +
                              "No se inserto el NumeroInterior de nacimiento correctamente" + usuario.Direccion.NumeroInterior +
                            "No se inserto la Colonia de nacimiento correctamente" + usuario.Direccion.Colonia +
                            "No se inserto el IdColonia correctamente" + usuario.Direccion.Colonia.IdColonia);
                    }
                    if (result.Correct)
                    {
                        result.Message = "Usuario Agregado correctamente";
                    }
                    else
                    {
                        result.Message = "Usuario no agregado";
                    }
                }
                if (resultErrores.Objects != null)
                {
                    TextWriter tx = new StreamWriter(@"C:\Users\digis\Documents\Jaime Corriente Romero\JCorrienteProgramacionNCapasCore\LayoutErrores.txt");
                    foreach (string error in resultErrores.Objects)
                    {
                        tx.WriteLine(error);
                    }
                    tx.Close();
                }
            }
            return PartialView("Modal");
        }
        [HttpPost]
        public ActionResult UsuarioCargaMasiva(ML.Usuario usuario)
        {

            IFormFile excelCargaMasiva = Request.Form.Files["FileExcel"];
            //Session 

            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                //Validacion de Excel
                if (excelCargaMasiva.Length > 0)
                {
                    // que sea .xlsx
                    string fileName = Path.GetFileName(excelCargaMasiva.FileName);
                    string folderPath = _configuration["PathFolder:value"];
                    string extensionArchivo = Path.GetExtension(excelCargaMasiva.FileName).ToLower();
                    string extensionModulo = _configuration["TipoExcel"];

                    if (extensionArchivo == extensionModulo)
                    {
                        //crear copia
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                excelCargaMasiva.CopyTo(stream);
                            }
                            //leer
                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            //convertir 
                            ML.Result resultConvertExcel = BL.Usuario.ConvertirExceltoDataTable(connectionString);
                            if (resultConvertExcel.Correct)
                            {
                                ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultConvertExcel.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }
                                return View("CargaMasiva",resultValidacion);
                            }
                            else
                            {
                                //error al leer el archivo
                                ViewBag.Message = "Ocurrio un error al leer el arhivo";
                                return View("Modal");
                            }
                        }
                    }

                }
                //verificar que no tenga errores 
                //le avisamos al usuario que el excel esta mal ML.ErrorExcel 
                //crea la sesion 

            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Usuario.ConvertirExceltoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {
                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el Usuario con nombre: " + usuarioItem.Nombre + " Error: " + resultAdd.Ex);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        string fileError = _hostingEnvironment.WebRootPath + @"~\Files\LogErrores.txt";
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los Usuarios No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los Usuarios han sido registrados correctamente";
                    }
                }
            }
            return PartialView("Modal");
        }
    }
}
