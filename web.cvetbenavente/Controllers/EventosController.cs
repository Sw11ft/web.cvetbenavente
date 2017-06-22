using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.cvetbenavente.Controllers
{
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext db;
        public EventosController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Modelos = new SelectList(Enum.GetNames(typeof(Enums.Modelos)));
            return View();
        }

        public IActionResult Importar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.DataCriacao = DateTime.UtcNow;
                evento.Id = Guid.NewGuid();

                db.Eventos.Add(evento);
                db.SaveChanges();

                return RedirectToAction("Index", new { nt = "s", nid = "6" });
            }
            else
            {
                return View(evento);
            }
        }
    }
}