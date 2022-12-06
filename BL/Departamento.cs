using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JcorrienteProgramacionNcapasContext context = new DL.JcorrienteProgramacionNcapasContext())
                {
                    var query = context.Departamentos.FromSqlRaw("DepartamentoGetAll").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var row in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();

                            departamento.IdDepartamento = row.IdDepartamento;
                            departamento.Nombre = row.Nombre;

                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = row.IdArea.Value;


                            result.Objects.Add(departamento); //boxing y unboxing

                        }

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
