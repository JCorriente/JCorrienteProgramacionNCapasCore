using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int IdMunicipio)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Colonia.FromSqlRaw($"ColoniaGetByIdMunicipio {IdMunicipio}").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var row in query)
                        {
                            ML.Colonia colonia = new ML.Colonia();

                            colonia.IdColonia = row.IdColonia;
                            colonia.Nombre = row.Nombre;
                            colonia.CodigoPostal = row.CodigoPostal;

                            colonia.Municipio = new ML.Municipio();
                            colonia.Municipio.IdMunicipio = IdMunicipio;

                            result.Objects.Add(colonia); //boxing y unboxing

                        }
                        result.Correct = true;
                    }
                }
                result.Correct = true;
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
