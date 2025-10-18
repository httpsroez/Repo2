using System;
using System.Collections.Generic;

namespace FormatoVacaciones.Models.Entities;

public partial class Solicitudvacacion
{
    public int IdSolicitudVacacion { get; set; }

    public int IdUsuario { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string? Comentarios { get; set; }

    public int IdEstado { get; set; }

    public int Numperiodo { get; set; }

    public string Año { get; set; } = null!;

    public int IdPersonaCubre { get; set; }

    public DateOnly FechaSolicitud { get; set; }

    public virtual EstadoSolicitud IdEstadoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
