using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.cvetbenavente.Data;
using Microsoft.AspNetCore.Authorization;
using static web.cvetbenavente.Models.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class XHRController : Controller
    {
        private readonly ApplicationDbContext db;

        public XHRController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: /<controller>/
        public IActionResult Registos(bool clientes = true, bool animais = true, bool especies = true)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") {
                int? nrClientes = (clientes) ? db.Clientes.Where(x => x.Active).Count() : (int?)null;
                int? nrAnimais = (animais) ? db.Animais.Where(x => x.Cliente.Active).Count() : (int?)null;
                int? nrEspecies = (especies) ? db.Especies.Where(x => x.Active).Count() : (int?)null;

                return Json(new { clientes = nrClientes, animais = nrAnimais, especies = nrEspecies });
            }

            return Json(new { error = true, message = "O pedido não foi efetuado através de XHR." });
        }

        public IActionResult Generos()
        {
            var generos = new Dictionary<int, string>();
            foreach (var item in Enum.GetValues(typeof(Genero)))
            {
                generos.Add((int)item, item.ToString());
            }

            return Json(generos);
        }
    }
}
