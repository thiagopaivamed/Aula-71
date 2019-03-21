using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Autenticacao.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Autenticacao.Controllers
{
    public class HomeController : Controller
    {
        private readonly Contexto _contexto;        
        public HomeController(Contexto contexto)
        {
            _contexto = contexto;
           
        }

        public IActionResult Index()
        {
            return View();
        }       
        
        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Pessoa p)
        {
            if (ModelState.IsValid)
            {
                _contexto.Pessoas.Add(p);
                await _contexto.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(p);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel dadosLogin)
        {
            if (_contexto.Pessoas.Any(x => x.Nome == dadosLogin.Nome && x.Senha == dadosLogin.Senha))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, dadosLogin.Nome)
                };

                var usuarioIdentidade = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentidade);

                await HttpContext.SignInAsync(principal);

                return View("Autenticado");
            }
            else return NotFound();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
