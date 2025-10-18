using System;
using System.Collections.Generic;

namespace FormatoVacaciones.Models.Entities;

public partial class EstadoSolicitud
{
    public int IdEstado { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Solicitudvacacion> Solicitudvacacion { get; set; } = new List<Solicitudvacacion>();
}
