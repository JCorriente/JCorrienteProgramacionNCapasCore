using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.ComponentModel;

namespace ML
{
    public class Usuario
    {
        public int Matricula { get; set; }

        //[Required]
        //[StringLength(20)]
        public string? Nombre { get; set; }

        //[Required]
        //[DisplayName("Apellido Paterno: ")]
        public string? ApellidoPaterno { get; set; }

        //[Required]
        //[DisplayName("Apellido Materno: ")]
        public string? ApellidoMaterno { get; set; }

        //[Required]
        //[DisplayName("Fecha de Nacimiento: ")]
        public string? FechaNacimiento { get; set; }

        //[Required]
        public string? Genero { get; set; }

        //[Required]
        //[DisplayName("Correo Electronico: ")]
        public string? Email { get; set; }

        //[Required]
        //[StringLength(12)]
        public string? Password { get; set; }

        //[Required]
        //[StringLength(10)]
        public string? Telefono { get; set; }

        //[Required]
        //[StringLength(10)]
        public string? Celular { get; set; }

        //[Required]
        //[StringLength(13)]
        public string? CURP { get; set; }

        //[Required]
        //[DisplayName("Nombre de Usuario: ")]
        public string? UserName { get; set; }

        public string  Imagen { get; set; }
        public bool Status { get; set; }

        public List<object> Usuarios { get; set; }

        //propiedad de navegacion
        public ML.Direccion Direccion { get; set; }
        public ML.Rol Rol { get; set; }
        
  
    }
}
