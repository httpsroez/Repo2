using FormatoVacaciones.Models.Entities;
using Microsoft.CodeAnalysis;

namespace FormatoVacaciones.Models.ViewModels
{
    public class DatosPersonal
    {
        public string PrimeraLetra { get; set; }
        public string ApellidoPrimeraLetra { get; set; }
        public string Nombre { get; set; } = null!;
        public string Puesto { get; set; } = null!;
        public string RpRt { get; set; } = null!;

        public IEnumerable<VacacionesModel> listitadevacaciones = null!;
        public IEnumerable<VacacionesModel> listitadevacacionesaprovadas = null!;


        public int año { get; set; }

        public DateOnly FechaDeIngreso { get; set; }

        public int? IdjefeDirecto { get; set; }

        public int Estado { get; set; }

        public int IdRol { get; set; }

        public string NombreDepartamento { get; set; }

        public int? DiasDisponibles { get; set; }

        public int DiasPendt { get; set; }
        public int DiasAprov { get; set; }
        public int Periodos { get; set; } = 4;


        public int? Antiguedad { get; set; }
    }
    public class VacacionesModel
    {
        public int IdSolicitudVacacion { get; set; }

        public int IdUsuario { get; set; }

        public DateOnly FechaInicio { get; set; }

        public DateOnly FechaFin { get; set; }
        public DateOnly HoySoli { get; set; }

        public string? Comentarios { get; set; }

        public int IdEstado { get; set; }

        public int? IdVacacion { get; set; }
        public int dias { get; set; }

    }
    //public class AntiguedadModel
    //{
    //    public IEnumerable<DateOnly> DiasSalteados { get; set; }
    //    public int? Antiguedad { get; set; }
    //    public DateOnly FechaInicio { get; set; }
    //    public DateOnly FechaFin { get; set; }

    //    public DateOnly FechaInicio2 { get; set; }
    //    public DateOnly FechaFin2 { get; set; }

    //    public DateOnly FechaInicio3 { get; set; }
    //    public DateOnly FechaFin3 { get; set; }

    //    public DateOnly FechaInicio4 { get; set; }
    //    public DateOnly FechaFin4 { get; set; }

    //}
}
