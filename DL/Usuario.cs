using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int Matricula { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Genero { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public string? Curp { get; set; }

    public string? UserName { get; set; }

    public byte? IdRol { get; set; }

    public string? Imagen { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual Rol? IdRolNavigation { get; set; }

    //Agregadas
    public string RolNombre { get; set; }
    public int IdDireccion { get; set; }
    public string Calle { get; set; }
    public string NumeroInterior { get; set; }
    public string NumeroExterior { get; set; }
    public int IdColonia { get; set; }
    public string ColoniaNombre { get; set; }
    public string CodigoPostal { get; set; }
    public int IdMunicipio { get; set; }
    public string MunicipioNombre { get; set; }
    public int IdEstado { get; set; }
    public string EstadoNombre { get; set; }
    public int IdPais { get; set; }
    public string PaisNombre { get; set; }
}
