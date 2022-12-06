using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    

                    producto.Departamento.IdDepartamento = (producto.Departamento.IdDepartamento == null) ? 0 : producto.Departamento.IdDepartamento;
                    
                    var productos = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}',{producto.Departamento.IdDepartamento}").ToList();
                    result.Objects = new List<object>();

                    if (productos != null)
                    {

                        foreach (var row in productos)
                        {
                            ML.Producto productoobj = new ML.Producto();

                            productoobj.IdProducto = row.IdProducto;
                            productoobj.Nombre = row.Nombre;
                            productoobj.PrecioUnitario = row.PrecioUnitario.Value;
                            productoobj.Stock = row.Stock.Value;
                            productoobj.Imagen = row.Imagen;
                            productoobj.Descripcion = row.Descripcion;
                            
                            
                            productoobj.Proveedor = new ML.Proveedor();
                            productoobj.Proveedor.IdProveedor = row.IdProveedor.Value;
                            productoobj.Proveedor.Nombre = row.NombreProveedor;
                      
                            

                            productoobj.Departamento = new ML.Departamento();
                            productoobj.Departamento.IdDepartamento = row.IdDepartamento.Value;
                            productoobj.Departamento.Nombre = row.NombreDepartamento;
                            
                          

                            productoobj.Area = new ML.Area();
                            productoobj.Area.IdArea = row.IdArea;
                            productoobj.Area.Nombre = row.NombreArea;                       

                            result.Objects.Add(productoobj); //boxing y unboxing

                        }
                        result.Correct = true;
                    }
                    else
                    {

                        result.Correct = false;
                        result.Message = "Ocurrio un error al realizar la consulta";
                    }
                }
                
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al realizar la consulta" + result.Ex;                
            }
            return result;
        }

        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}','{producto.PrecioUnitario}',{producto.Stock},{producto.Proveedor.IdProveedor},{producto.Departamento.IdDepartamento},'{producto.Descripcion}',{producto.Area.IdArea},'{producto.Imagen}'");

                    if (query > 0)
                    {
                        result.Message = "Se agrego el producto correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "Ocurrio un error al insertar el Producto" + result.Ex;
                    }
                }
                result.Correct = true;
            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el Producto" + result.Ex;

            }//manejo de excepciones 
            return result;

        }
        public static ML.Result GetById(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetById {IdProducto}").AsEnumerable().SingleOrDefault();
                   
                    if (query != null)
                    {

                        ML.Producto producto = new ML.Producto();

                        producto.IdProducto = query.IdProducto;
                        producto.Nombre = query.Nombre;
                        producto.PrecioUnitario = query.PrecioUnitario.Value;
                        producto.Stock = query.Stock.Value;
                        producto.Descripcion = query.Descripcion;
                        producto.Imagen = query.Imagen;


                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = query.IdProveedor.Value;
                        producto.Proveedor.Nombre = query.NombreProveedor;

                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = query.IdDepartamento.Value;
                        producto.Departamento.Nombre = query.NombreDepartamento;


                        producto.Area = new ML.Area();
                        producto.Area.IdArea = query.IdArea;
                        producto.Area.Nombre = query.NombreArea;

                        result.Object = producto;//boxing y unboxing
                        result.Correct = true;
                    }

                }
               

            }//codigo que puede causar una excepcion 
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "Ocurrio un error al insertar el producto" + result.Ex;


            }//manejo de excepciones 

            return result;
        }

        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto},{producto.PrecioUnitario},{producto.Stock},{producto.Proveedor.IdProveedor},{producto.Departamento.IdDepartamento},'{producto.Descripcion}',{producto.Area.IdArea},'{producto.Imagen}'");

                    if (query > 0)
                    {
                        result.Message = "Se actualizo el producto correctamente";
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
                result.Message = "Ocurrio un error al actulizar el producto" + result.Ex;

            }//manejo de excepciones 
            return result;

        }

    }
}
