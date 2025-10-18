using System;
using System.Collections.Generic;

namespace FormatoVacaciones.Models.Entities;

public partial class Puesto
{
    public int IdPuesto { get; set; }

    public string NombrePuesto { get; set; } = null!;

    public int Estado { get; set; }

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
