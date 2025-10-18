using System.Security.Policy;

namespace FormatoVacaciones.Models.ViewModels
{
    public class PeriodosVacacionesViewModel
    {
        public int? TotalDiasDisponibles { get; set; }
        public IEnumerable<DatosVacacionesModel> ListaDeVacacionesPorPeriodo { get; set; }
        public IEnumerable<PersonalModel> ListaPersonal { get; set; } = null!;


        public int IdCubre1 { get; set; }
        public int IdCubre2 { get; set; }
        public int IdCubre3 { get; set; }
        public int IdCubre4 { get; set; }

        //////////////////////////////////////////////////////////////////////////////////////////
        public DateOnly? APrimerPeriodoFechaInicio { get; set; }
        public DateOnly? BPrimerPeriodoFechaFin { get; set; }
        public DateOnly? CPrimerPeriodoFechaInicio { get; set; }
        public DateOnly? DPrimerPeriodoFechaFin { get; set; } 
        public DateOnly? EPrimerPeriodoFechaInicio { get; set; }
        public DateOnly? FPrimerPeriodoFechaFin { get; set; }
        public DateOnly? GPrimerPeriodoFechaInicio { get; set; }
        public DateOnly? HPrimerPeriodoFechaFin { get; set; }
    }
    public class DatosVacacionesModel
    {
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int Numperiodo { get; set; }
        public sbyte TotalHabil { get; set; }
        public sbyte DiasDisponibles { get; set; }
        public string? Comentarios { get; set; }
        public int? PersonaCubre { get; set; }
        
    }
    public class PersonalModel
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? RPE { get; set; }
        public string? Departamenteo { get; set; }
    }
}
