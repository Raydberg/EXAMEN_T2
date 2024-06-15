using EXAMEN_T2.Datos;
using EXAMEN_T2.Models;
using EXAMEN_T2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EXAMEN_T2.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDbContext _contexto;
        public AccesoController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        {
            if (modelo.Clave != modelo.ConfirmarClave)
            {
                ViewData["mensaje"] = "Las claves no coinciden";
                return View();
            }
            Usuario u = new Usuario()
            {
                NombreCompleto = modelo.NombreCompleto,
                Correo = modelo.Correo,
                Clave = modelo.Clave
            };
            await _contexto.Usuarios.AddAsync(u);
            await _contexto.SaveChangesAsync();
            if (u.IdUsuario!=0) return RedirectToAction("Login","Acceso");
            ViewData["mensaje"] = "No se pudo crear el usuario";
            return View();
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginVM modelo)
        {
            Usuario? usuario_encontrado = await _contexto.Usuarios.Where(u =>
                                                                        u.Correo == modelo.Correo &&
                                                                        u.Clave == modelo.Clave).FirstOrDefaultAsync();
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontro coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,usuario_encontrado.NombreCompleto )
        };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties propiedades = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), propiedades);

            return RedirectToAction("Index", "Home");
        }




    }
}
