using FormatoVacaciones.Models.Entities;
using gpdSW.repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormatoVacaciones.Controllers
{
    public class AccountController : Controller
    {
        VacacionescfepruebaContext context;
        Repository<Usuario> usuarioRepository;
        public AccountController()
        {
            context= new VacacionescfepruebaContext();
            usuarioRepository = new Repository<Usuario>(context);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.RpRt))
                ModelState.AddModelError("", "Ingrese su correo.");
            if (string.IsNullOrWhiteSpace(vm.Password))
                ModelState.AddModelError("", "Ingrese su contraseña.");
            if (ModelState.IsValid)
            {
                var user = usuarioRepository.GetAll().Where(x => x.RpRt == vm.RpRt && x.Password == vm.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "El nombre o contraseña son incorrectos");
                    return View(vm);
                    
                }

                // Claims
                List<Claim> claims =
                [
                    new Claim("Id", user.IdUsuario.ToString()),
                    new Claim("Nombre", user.Nombre),
                    new Claim("Rpm", user.RpRt),
                    new Claim("Rol",user.IdRol.ToString())


                ];

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);

                this.HttpContext.SignInAsync(principal);


                return RedirectToAction("Personal", "Personal");

            }
            return View(vm);
        }

    }
}
