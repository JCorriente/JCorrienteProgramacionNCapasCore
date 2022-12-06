using System;
using System.Collections.Generic;

namespace DL;

public partial class Area
{
    public int IdArea { get; set; }

    public string? Nombre { get; set; } = null!;

    public virtual ICollection<Departamento> Departamentos { get; } = new List<Departamento>();

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
