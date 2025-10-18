using FormatoVacaciones.Models.Entities;
using FormatoVacaciones.Models.ViewModels;
using gpdSW.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormatoVacaciones.Controllers
{
    public class HomeController : Controller
    {
        VacacionescfepruebaContext context;
        Repository<Usuario> usuarioRepository;
        public HomeController()
        {
            context = new VacacionescfepruebaContext();
            usuarioRepository = new Repository<Usuario>(context);
        }
        public IActionResult Index()
        {

            return View();
        }

        
    }
}
