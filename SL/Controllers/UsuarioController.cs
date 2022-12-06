using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/<AlumnoController>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = new ML.Result();

            ML.Result resultRol = BL.Rol.GetAll();
            result = BL.Usuario.GetAll(usuario);


            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("GetAll")]
        public IActionResult GetAll(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();
            ML.Result resultRol = BL.Rol.GetAll();
            result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
           
        }


        // GET api/<AlumnoController>/5
        [HttpGet("GetById/{Matricula}")]
        public IActionResult GetById(int Matricula)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            ML.Result result = BL.Usuario.GetById(Matricula);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        //// POST api/<UsuarioController>
        [HttpPost("add")]
        public IActionResult Post([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

            //// PUT api/<AlumnoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // delete api/<alumnocontroller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
