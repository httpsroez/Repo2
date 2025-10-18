namespace FormatoVacaciones.Models.ViewModels
{
    public class ProgramarVacacionesViewModel
    {
        public IEnumerable<ListaEmpleados> listitaempleados { get; set; } = null!;

    }
    public class ListaEmpleados
    {
        public string Nombre { get; set; } = null!;
        public string? NombreDepartamento { get; set; }
    }
}
