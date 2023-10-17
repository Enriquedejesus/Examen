using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Security.Claims;

namespace WebApplication1.Models
{
    public class UsuariosController : Controller
    {
            private readonly DispositivoDBContext context;

        public UsuariosController(DispositivoDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios u)
        {

            if (ModelState.IsValid)
            {
                Usuarios utemp = context.Usuarios.Include(x => x.Roles)
                                .FirstOrDefault(x => x.Usuario == u.Usuario
                                && x.Password == u.Password)!;
                if (utemp != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("username", utemp.Usuario!));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, utemp.Usuario!));
                    claims.Add(new Claim(ClaimTypes.Role, utemp.Roles.Rol));
                    ClaimsIdentity identity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Dispositivo");
                }
            }
            ViewBag.Error = "Usuario y/o password incorrectos";
            return View(u);
        }

        public IActionResult NoPermitido()
        {
            return View();
        }

        public async Task<IActionResult> CerrarSesion(){
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
            


    }
}