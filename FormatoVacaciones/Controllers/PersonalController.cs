using System.Linq;
using FormatoVacaciones.Models.Entities;
using FormatoVacaciones.Models.ViewModels;
using gpdSW.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormatoVacaciones.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        VacacionescfepruebaContext context;
        Repository<Usuario> usuarioRepository;
        Repository<Solicitudvacacion> solicitudvacacionesRepository;
        public PersonalController()
        {
            context = new VacacionescfepruebaContext();
            usuarioRepository = new Repository<Usuario>(context);
            solicitudvacacionesRepository = new(context);


        }

        public IActionResult Personal()
        {
            var IdUser = User.FindFirst("Id")?.Value;

            DatosPersonal vm = new();
            DateTime fechahoy = DateTime.Now;

            var diasaprovados = solicitudvacacionesRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser) && x.IdEstado == 3).Select(x => x.FechaFin.Day - x.FechaInicio.Day).FirstOrDefault();
            var nombre1 = usuarioRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser)).Select(x => x.Nombre).FirstOrDefault();

            string primerCaracterDespuesDelEspacio = nombre1.Substring(nombre1.IndexOf(" ") + 1, 1);
            string primerCaracter = nombre1.Substring(nombre1.IndexOf("") + 0, 1);

            var diaspendt = solicitudvacacionesRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser) && x.IdEstado == 2).Select(x => x.FechaInicio.Day + (x.FechaFin.Day - 1)).FirstOrDefault();


            var datos = context.Usuario.Where(x => x.IdUsuario == Convert.ToInt32(IdUser)).Select(x => new DatosPersonal
            {
                PrimeraLetra = primerCaracter,
                ApellidoPrimeraLetra = primerCaracterDespuesDelEspacio,
                Nombre = x.Nombre,
                FechaDeIngreso = x.FechaDeIngreso,
                NombreDepartamento = x.IdDepartamentoNavigation.NombreDepartamento,
                Puesto = x.IdPuestoNavigation.NombrePuesto,
                Antiguedad = x.Antiguedad,
                RpRt = x.RpRt,
                IdjefeDirecto = x.IdjefeDirecto,
                Estado = x.Estado,
                IdRol = x.IdRol,
                año = DateTime.Now.Year,
                DiasAprov = diasaprovados,
                DiasDisponibles = x.DiasDisponibles - diasaprovados,
                DiasPendt = diaspendt,


                listitadevacaciones = x.Solicitudvacacion.Where(x => x.IdUsuarioNavigation.IdUsuario == Convert.ToInt32(IdUser)).Select(x => new VacacionesModel
                {
                    Comentarios = x.Comentarios,
                    FechaFin = x.FechaFin,
                    FechaInicio = x.FechaInicio,
                    IdEstado = x.IdEstado,
                    IdSolicitudVacacion = x.IdSolicitudVacacion,
                    IdUsuario = x.IdUsuario,
                    dias = x.FechaFin.Day - x.FechaInicio.Day,
                    HoySoli = x.FechaSolicitud,


                }).ToList(),
                listitadevacacionesaprovadas = x.Solicitudvacacion.Where(x => x.IdUsuarioNavigation.IdUsuario == Convert.ToInt32(IdUser) && x.IdEstado == 3)
                 .Select(x => new VacacionesModel
                 {
                     FechaInicio = x.FechaInicio,
                     FechaFin = x.FechaFin,
                 }).ToList()





            })
                .FirstOrDefault();



            //vm.listitadevacaciones = context.Solicitudvacacion.Select(x => new VacacionesModel
            //{
            //    IdUsuario = x.IdUsuario,
            //    FechaInicio = x.FechaInicio,
            //    FechaFin = x.FechaFin,
            //    Comentarios = x.Comentarios,
            //    IdEstado = x.IdEstado,
            //    IdSolicitudVacacion = x.IdSolicitudVacacion,
            //    IdVacacion = x.IdVacacion,



            //});







            return View(datos);
        }
        [HttpGet]
        public IActionResult ProgramarVacaciones()
        {
            var IdUser = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(IdUser))
            {
                return RedirectToAction("Login", "Account");
            }

            var diasaprovados = solicitudvacacionesRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser) && x.IdEstado == 3).Select(x => x.FechaInicio.Day + (x.FechaFin.Day - 1)).FirstOrDefault();
            var antiguedad = usuarioRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser)).Select(x => x.Antiguedad).FirstOrDefault();

            var vm = context.Usuario.Where(x => x.IdUsuario == Convert.ToInt32(IdUser)).Select(x => new PeriodosVacacionesViewModel
            {
                TotalDiasDisponibles = x.DiasDisponibles,

                ListaDeVacacionesPorPeriodo = x.Solicitudvacacion.Select(s => new DatosVacacionesModel
                {
                    Numperiodo = s.Numperiodo,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    TotalHabil = Convert.ToSByte(EF.Functions.DateDiffDay(s.FechaInicio, s.FechaFin) + 1),
                    Comentarios = s.Comentarios,
                    DiasDisponibles = Convert.ToSByte(x.DiasDisponibles),
                    PersonaCubre = s.IdPersonaCubre
                }).ToList(),
                ListaPersonal = context.Usuario.Where(x => x.IdUsuario != Convert.ToInt32(IdUser)).OrderBy(x => x.IdDepartamentoNavigation.NombreDepartamento).Select(x => new PersonalModel
                {
                    Id = x.IdUsuario,
                    Nombre = x.Nombre,
                    Departamenteo = x.IdDepartamentoNavigation.NombreDepartamento,
                    RPE = x.RpRt
                }).ToList()

            }).FirstOrDefault();


            if (vm == null)
            {
                vm = new PeriodosVacacionesViewModel
                {
                    TotalDiasDisponibles = 0,
                    ListaDeVacacionesPorPeriodo = new List<DatosVacacionesModel>()
                };
            }

            else if (vm.ListaDeVacacionesPorPeriodo == null)
            {
                vm.ListaDeVacacionesPorPeriodo = new List<DatosVacacionesModel>();
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult ProgramarVacaciones(PeriodosVacacionesViewModel vm)
        {
            ModelState.Clear();
            var IdUser = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(IdUser))
            {
                ModelState.AddModelError("", "Usuario no válido");
                return View(vm);
            }

            // Obtener el conteo actual de periodos
            var contparaerror = solicitudvacacionesRepository.GetAll().Where(x => x.IdUsuario == Convert.ToInt32(IdUser)).Count();

            if (contparaerror >= 4)
            {
                ModelState.AddModelError("", "YA CUENTAS CON 4 PERIODOS CREADOS");
                return View(ObtenerViewModelConDatos(Convert.ToInt32(IdUser)));
            }


            var solicitudesExistentes = context.Solicitudvacacion
                .Where(x => x.IdUsuario == Convert.ToInt32(IdUser))
                .ToList();

            if (vm.IdCubre1 != 0)
            {
                var Rango = solicitudvacacionesRepository
     .GetAll()
     .Where(x => x.IdPersonaCubre == vm.IdCubre1 && x.IdEstado == 3 &&
                 (
                     (x.FechaInicio >= vm.APrimerPeriodoFechaInicio && x.FechaInicio <= vm.BPrimerPeriodoFechaFin) ||
                     (x.FechaFin >= vm.APrimerPeriodoFechaInicio && x.FechaFin <= vm.BPrimerPeriodoFechaFin) ||
                     (x.FechaInicio <= vm.BPrimerPeriodoFechaFin && x.FechaFin >= vm.APrimerPeriodoFechaInicio)
                 )
     ).Count();
                if (Rango != 0)
                {
                    ModelState.AddModelError("", "LA PERSONA SELECCIONADA A CUBRIR TIENE FECHAS A CUBRIR");
                }

                var ProiasVacaiones = solicitudvacacionesRepository
       .GetAll()
       .Where(x => x.IdPersonaCubre == Convert.ToUInt32(IdUser) &&
                   x.IdEstado == 3 && // Solo solicitudes en estado pendiente
                   (
                       (x.FechaInicio >= vm.APrimerPeriodoFechaInicio && x.FechaInicio <= vm.BPrimerPeriodoFechaFin) ||
                       (x.FechaFin >= vm.APrimerPeriodoFechaInicio && x.FechaFin <= vm.BPrimerPeriodoFechaFin) ||
                       (x.FechaInicio <= vm.BPrimerPeriodoFechaFin && x.FechaFin >= vm.APrimerPeriodoFechaInicio)
                   )
       )
       .Count();

                if (ProiasVacaiones != 0)
                {
                    ModelState.AddModelError("", "TIENES FECHAS ASIGNADAS A CUBRIR");
                }

            }





            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.APrimerPeriodoFechaInicio.HasValue && vm.BPrimerPeriodoFechaFin.HasValue)
                    {

                        var existe = solicitudesExistentes.Any(x =>
                            x.FechaInicio == vm.APrimerPeriodoFechaInicio.Value &&
                            x.FechaFin == vm.BPrimerPeriodoFechaFin.Value);

                        if (!existe)
                        {
                            Solicitudvacacion vmp2 = new()
                            {

                                IdUsuario = Convert.ToInt32(IdUser),
                                FechaInicio = vm.APrimerPeriodoFechaInicio.Value,
                                FechaFin = vm.BPrimerPeriodoFechaFin.Value,
                                Comentarios = "Vacaciones De Verano",
                                IdEstado = 1,
                                Numperiodo = 1,
                                Año = DateTime.Now.Year.ToString(),
                                IdPersonaCubre = vm.IdCubre1,
                                FechaSolicitud = DateOnly.FromDateTime(DateTime.Now)


                            };
                            solicitudvacacionesRepository.Insert(vmp2);
                        }
                    }


                    if (vm.CPrimerPeriodoFechaInicio.HasValue && vm.DPrimerPeriodoFechaFin.HasValue)
                    {
                        var existe = solicitudesExistentes.Any(x =>
                            x.FechaInicio == vm.CPrimerPeriodoFechaInicio.Value &&
                            x.FechaFin == vm.DPrimerPeriodoFechaFin.Value);

                        if (!existe)
                        {
                            Solicitudvacacion vmp2 = new()
                            {
                                IdUsuario = Convert.ToInt32(IdUser),
                                FechaInicio = vm.CPrimerPeriodoFechaInicio.Value,
                                FechaFin = vm.DPrimerPeriodoFechaFin.Value,
                                Comentarios = "Vacaciones De Verano",
                                IdEstado = 1,
                                Numperiodo = 2,
                                Año = DateTime.Now.Year.ToString(),
                                IdPersonaCubre = vm.IdCubre2,
                                FechaSolicitud = DateOnly.FromDateTime(DateTime.Now)
                            };
                            solicitudvacacionesRepository.Insert(vmp2);
                        }
                    }


                    if (vm.EPrimerPeriodoFechaInicio.HasValue && vm.FPrimerPeriodoFechaFin.HasValue)
                    {
                        var existe = solicitudesExistentes.Any(x =>
                            x.FechaInicio == vm.EPrimerPeriodoFechaInicio.Value &&
                            x.FechaFin == vm.FPrimerPeriodoFechaFin.Value);

                        if (!existe)
                        {
                            Solicitudvacacion vmp2 = new()
                            {
                                IdUsuario = Convert.ToInt32(IdUser),
                                FechaInicio = vm.EPrimerPeriodoFechaInicio.Value,
                                FechaFin = vm.FPrimerPeriodoFechaFin.Value,
                                Comentarios = "Vacaciones De Verano",
                                IdEstado = 1,
                                Numperiodo = 3,
                                Año = DateTime.Now.Year.ToString(),
                                IdPersonaCubre = vm.IdCubre3,
                                FechaSolicitud = DateOnly.FromDateTime(DateTime.Now)

                            };
                            solicitudvacacionesRepository.Insert(vmp2);
                        }
                    }


                    if (vm.GPrimerPeriodoFechaInicio.HasValue && vm.HPrimerPeriodoFechaFin.HasValue)
                    {
                        var existe = solicitudesExistentes.Any(x =>
                            x.FechaInicio == vm.GPrimerPeriodoFechaInicio.Value &&
                            x.FechaFin == vm.HPrimerPeriodoFechaFin.Value);

                        if (!existe)
                        {
                            Solicitudvacacion vmp2 = new()
                            {
                                IdUsuario = Convert.ToInt32(IdUser),
                                FechaInicio = vm.GPrimerPeriodoFechaInicio.Value,
                                FechaFin = vm.HPrimerPeriodoFechaFin.Value,
                                Comentarios = "Vacaciones De Verano",
                                IdEstado = 1,
                                Numperiodo = 4,
                                Año = DateTime.Now.Year.ToString(),
                                IdPersonaCubre = vm.IdCubre4,
                                FechaSolicitud = DateOnly.FromDateTime(DateTime.Now)
                            };
                            solicitudvacacionesRepository.Insert(vmp2);
                        }
                    }

                    return RedirectToAction("Personal", "Personal");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al guardar las solicitudes: {ex.Message}");
                }
            }


            return View(ObtenerViewModelConDatos(Convert.ToInt32(IdUser)));
        }

        private PeriodosVacacionesViewModel ObtenerViewModelConDatos(int idUsuario)
        {
            return context.Usuario
                .Where(x => x.IdUsuario == idUsuario)
                .Select(x => new PeriodosVacacionesViewModel
                {
                    TotalDiasDisponibles = x.DiasDisponibles,
                    ListaDeVacacionesPorPeriodo = x.Solicitudvacacion.Select(s => new DatosVacacionesModel
                    {
                        Numperiodo = s.Numperiodo,
                        FechaInicio = s.FechaInicio,
                        FechaFin = s.FechaFin,
                        TotalHabil = Convert.ToSByte(EF.Functions.DateDiffDay(s.FechaInicio, s.FechaFin) + 1),
                        Comentarios = s.Comentarios,
                        DiasDisponibles = Convert.ToSByte(x.DiasDisponibles),
                        PersonaCubre = s.IdPersonaCubre

                    }).ToList(),
                    ListaPersonal = context.Usuario.Select(x => new PersonalModel
                    {
                        Id = x.IdUsuario,
                        Nombre = x.Nombre,
                        Departamenteo = x.IdDepartamentoNavigation.NombreDepartamento,
                        RPE = x.RpRt
                    }).ToList()

                }).FirstOrDefault() ?? new PeriodosVacacionesViewModel();
        }


    }
}
