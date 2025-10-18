using System;
using System.Collections.Generic;

namespace FormatoVacaciones.Models.Entities;

public partial class Vacaciones
{
    public int Idvacacion { get; set; }

    public int Periodos { get; set; }

    public DateOnly Vigencia { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
