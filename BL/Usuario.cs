using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    usuario.Rol.IdRol = (usuario.Rol.IdRol == null) ? 0 : usuario.Rol.IdRol;
                    var usuarios = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}', '{usuario.ApellidoPaterno}', {usuario.Rol.IdRol}").ToList();
                    result.Objects = new List<object>();
                    if (usuarios != null)
                    {

                        foreach (var objUsuario in usuarios)
                        {
                            ML.Usuario usuarioobj = new ML.Usuario();
                            usuarioobj.Matricula = objUsuario.Matricula;
                            usuarioobj.Nombre = objUsuario.Nombre;
                            usuarioobj.ApellidoPaterno = objUsuario.ApellidoPaterno;
                            usuarioobj.ApellidoMaterno = objUsuario.ApellidoMaterno;
                            usuarioobj.FechaNacimiento = objUsuario.FechaNacimiento.Value.ToString();
                            usuarioobj.Genero = objUsuario.Genero;
                            usuarioobj.Email = objUsuario.Email;
                            usuarioobj.Password = objUsuario.Password;
                            usuarioobj.Telefono = objUsuario.Telefono;
                            usuarioobj.Celular = objUsuario.Celular;
                            usuarioobj.CURP = objUsuario.Curp;
                            usuarioobj.UserName = objUsuario.UserName;
                            usuarioobj.Imagen = objUsuario.Imagen;
                            usuarioobj.Status = objUsuario.Status;


                            usuarioobj.Rol = new ML.Rol();
                            usuarioobj.Rol.IdRol = objUsuario.IdRol;
                            usuarioobj.Rol.Nombre = objUsuario.RolNombre;

                            usuarioobj.Direccion = new ML.Direccion();
                            usuarioobj.Direccion.IdDireccion = objUsuario.IdDireccion;
                            usuarioobj.Direccion.Calle = objUsuario.Calle;
                            usuarioobj.Direccion.NumeroInterior = objUsuario.NumeroInterior;
                            usuarioobj.Direccion.NumeroExterior = objUsuario.NumeroExterior;

                            usuarioobj.Direccion.Colonia = new ML.Colonia();
                            usuarioobj.Direccion.Colonia.IdColonia = objUsuario.IdColonia;
                            usuarioobj.Direccion.Colonia.Nombre = objUsuario.ColoniaNombre;
                            usuarioobj.Direccion.Colonia.CodigoPostal = objUsuario.CodigoPostal;

                            usuarioobj.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuarioobj.Direccion.Colonia.Municipio.IdMunicipio = objUsuario.IdMunicipio;
                            usuarioobj.Direccion.Colonia.Municipio.Nombre = objUsuario.MunicipioNombre;

                            usuarioobj.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuarioobj.Direccion.Colonia.Municipio.Estado.IdEstado = objUsuario.IdEstado;
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Nombre = objUsuario.EstadoNombre;

                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais.IdPais = objUsuario.IdPais;
                            usuarioobj.Direccion.Colonia.Municipio.Estado.Pais.Nombre = objUsuario.PaisNombre;

                            result.Objects.Add(usuarioobj); //boxing y unboxing
                            
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se ha podido realizar la consulta";

                    }
                } 
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el alumno" + result.Ex;
                      
            }
            return result;
        }
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}','{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}','{usuario.FechaNacimiento}','{usuario.Genero}','{usuario.Email}','{usuario.Password}','{usuario.Telefono}','{usuario.Celular}','{usuario.CURP}','{usuario.UserName}',{usuario.Rol.IdRol},'{usuario.Imagen}','{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}','{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia},{usuario.Status}");

                    if (query > 0)
                    {
                        result.Message = "Se agrego el usuario correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "Ocurrio un error al insertar el Usuario" + result.Ex;
                    }
                }
                result.Correct = true;
            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el Usuario" + result.Ex;
  
            }//manejo de excepciones 
            return result;

        }

        public static ML.Result GetById(int Matricula)  
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetById {Matricula}").AsEnumerable().SingleOrDefault();
                
                    if (query != null)
                    {

                        ML.Usuario usuario = new ML.Usuario();

                        usuario.Matricula = query.Matricula;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.FechaNacimiento = query.FechaNacimiento.Value.ToString();
                        usuario.Genero = query.Genero;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        usuario.CURP = query.Curp;
                        usuario.UserName = query.UserName;
                        usuario.Imagen = query.Imagen;
                        usuario.Status = query.Status;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;
                        usuario.Rol.Nombre = query.RolNombre;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = query.IdDireccion;
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = query.IdColonia;
                        usuario.Direccion.Colonia.Nombre = query.ColoniaNombre;
                        usuario.Direccion.Colonia.CodigoPostal = query.CodigoPostal;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = query.MunicipioNombre;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = query.EstadoNombre;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = query.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = query.PaisNombre;


                        result.Object = usuario;//boxing y unboxing
                        result.Correct = true;
                    }

                }
            

            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el alumno" + result.Ex;

            
            }//manejo de excepciones 

            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UpdateSP {usuario.Matricula},'{usuario.Nombre}','{usuario.ApellidoPaterno}','{usuario.ApellidoMaterno}','{usuario.FechaNacimiento}','{usuario.Genero}','{usuario.Email}','{usuario.Password}','{usuario.Telefono}','{usuario.Celular}','{usuario.CURP}','{usuario.UserName}',{usuario.Rol.IdRol},'{usuario.Imagen}','{usuario.Direccion.Calle}','{usuario.Direccion.NumeroInterior}','{usuario.Direccion.NumeroExterior}',{usuario.Direccion.Colonia.IdColonia}");

                    if (query > 0)
                    {
                        result.Message = "Se actualizo el usuario correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }
                //}
                result.Correct = true;
            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al actulizar el Usuario" + result.Ex;

            }//manejo de excepciones 
            return result;

        }

        public static ML.Result Delete(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DeleteSP {usuario.Matricula}");


                    if (query >= 0)
                    {
                        result.Message = "Se elimino el usuario correctamente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al eliminar el Usuario" + result.Ex;

            }
            return result;
        }

        public static ML.Result ConvertirExceltoDataTable(string connString)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (OleDbConnection context = new OleDbConnection(connString))
                {
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;

                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableUsuario = new DataTable();

                        da.Fill(tableUsuario);

                        if (tableUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();
                                usuario.Nombre = row[0].ToString();
                                usuario.ApellidoPaterno = row[1].ToString();
                                usuario.ApellidoMaterno = row[2].ToString();
                                usuario.FechaNacimiento = row[3].ToString();
                                usuario.Genero = row[4].ToString();
                                usuario.Email = row[5].ToString();
                                usuario.Password = row[6].ToString();
                                usuario.Telefono = row[7].ToString();
                                usuario.Celular = row[8].ToString();
                                usuario.CURP = row[9].ToString();
                                usuario.UserName = row[10].ToString();

                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = byte.Parse(row[11].ToString());
                                usuario.Rol.Nombre = row[12].ToString();

                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.IdDireccion = int.Parse(row[13].ToString());
                                usuario.Direccion.Calle = row[14].ToString();
                                usuario.Direccion.NumeroInterior = row[15].ToString();
                                usuario.Direccion.NumeroExterior = row[16].ToString();

                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(row[17].ToString());

                                result.Objects.Add(usuario);
                            }
                            result.Correct = true;
                        }
                        if (tableUsuario.Rows.Count > 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "Ocurrio un error al insertar el alumno" + result.Ex;

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el Usuario" + result.Ex;
            }
            return result;
        }

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();
            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Usuario usuario in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    usuario.Nombre = (usuario.Nombre == "") ? error.Mensaje += "Ingresar el nombre  " : usuario.Nombre; //operador ternario

                    if (usuario.ApellidoPaterno == "")
                    {
                        error.Mensaje += "Ingresar el Apellido Paterno ";
                    }
                    if (usuario.ApellidoMaterno == "")
                    {
                        error.Mensaje += "Ingresar el Apellido Materno ";
                    }
                    if (usuario.FechaNacimiento == "")
                    {
                        error.Mensaje += "Ingresar la Fecha de Nacimiento ";
                    }
                    if (usuario.Genero == "")
                    {
                        error.Mensaje += "Ingresar el Genero ";
                    }
                    if (usuario.Email == "")
                    {
                        error.Mensaje += "Ingresar el Email ";
                    }
                    if (usuario.Password == "")
                    {
                        error.Mensaje += "Ingresar el Password ";
                    }
                    if (usuario.Telefono == "")
                    {
                        error.Mensaje += "Ingresar el Telefeno ";
                    }
                    if (usuario.Celular == "")
                    {
                        error.Mensaje += "Ingresar el Mensaje ";
                    }
                    if (usuario.CURP == "")
                    {
                        error.Mensaje += "Ingresar el CURP ";
                    }
                    if (usuario.UserName == "")
                    {
                        error.Mensaje += "Ingresar el UserName ";
                    }
                    if (usuario.Rol.IdRol.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Rol ";
                    }
                    if (usuario.Direccion.IdDireccion.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Id de Direccion ";
                    }
                    if (usuario.Direccion.Calle.ToString() == "")
                    {
                        error.Mensaje += "Ingresar la Calle ";
                    }
                    if (usuario.Direccion.NumeroInterior.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Numero Interior ";
                    }
                    if (usuario.Direccion.NumeroExterior.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Numero Exterior ";
                    }
                    if (usuario.Direccion.Colonia.IdColonia.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el Id de Colonia ";
                    }
                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el Usuario" + result.Ex;

            }
            return result;
        }

        public static ML.Result ChangeStatus(int matricula, bool status)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var usuarios = context.Database.ExecuteSqlRaw($"UsuarioChangeStatus {matricula}, {status}");

                    if (usuarios > 0)
                    {
                        result.Correct = true;
                    }
          
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error " + result.Ex;
            }
            return result;
        }


    }
}
