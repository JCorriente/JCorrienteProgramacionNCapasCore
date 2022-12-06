using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Estado
    {
        public static ML.Result GetByIdPais(int IdPais)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Estados.FromSqlRaw($"EstadoGetByIdPais {IdPais}").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var row in query)
                        {
                            ML.Estado estado = new ML.Estado();

                            estado.IdEstado = row.IdEstado;
                            estado.Nombre = row.Nombre;

                            estado.Pais = new ML.Pais();
                            estado.Pais.IdPais = IdPais;

                            result.Objects.Add(estado); //boxing y unboxing

                        }
                        result.Correct = true;
                    }
                }
               
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = "No se ha podido realizar la consulta" + result.Ex;             
            }
            return result;
        }

    }
}
