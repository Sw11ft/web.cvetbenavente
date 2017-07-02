using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext db;
        public EventosController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index(string from = null,
                                   string to = null,
                                   string cl = null,
                                   string esp = null,
                                   string animal = null)
        {
            var culture = new CultureInfo("pt-PT");

            if (!DateTime.TryParseExact(from, "d/M/yyyy", culture, DateTimeStyles.None, out DateTime fromDate))
            {
                fromDate = DateTime.UtcNow;
            }

            if (!DateTime.TryParseExact(to, "d/M/yyyy", culture, DateTimeStyles.None, out DateTime toDate))
            {
                toDate = DateTime.UtcNow.AddYears(1);
            }

            Models.EventosViewModels.IndexViewModel model = new Models.EventosViewModels.IndexViewModel();

            var eventos = db.Eventos
                .Where(x => x.Data != null)
                .Where(x => x.Data > fromDate)
                .Where(x => x.Data <= toDate)
                .Where(x => x.Cliente.Active)
                .Where(x => !x.Animal.Removido)
                .Include(x => x.Cliente).Include(x => x.Animal)
                .Include(x => x.Animal.Especie)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(cl))
            {
                eventos = eventos.Where(x => x.IdCliente.ToString() == cl);
            }

            if (!string.IsNullOrWhiteSpace(esp))
            {
                eventos = eventos.Where(x => x.Animal.IdEspecie.ToString() == esp);
            }

            if (!string.IsNullOrWhiteSpace(animal))
            {
                eventos = eventos.Where(x => x.IdAnimal.ToString() == animal);
            }

            eventos = eventos.OrderBy(x => x.Data);

            List<Evento> eventosList = eventos.ToList();

            foreach (var item in eventosList)
            {
                if (!model.Anos.Where(x => x.Valor == item.Data.Value.Year).Any())
                {
                    model.Anos.Add(new Models.EventosViewModels.IndexViewModel.Ano
                    {
                        Valor = item.Data.Value.Year
                    });
                }

                if (!model.Anos.Where(x => x.Valor == item.Data.Value.Year).First().Meses
                        .Where(x => x.Valor == item.Data.Value.Month).Any())
                {
                    model.Anos.Where(x => x.Valor == item.Data.Value.Year).First()
                        .Meses.Add(new Models.EventosViewModels.IndexViewModel.Mes
                        {
                            Valor = item.Data.Value.Month,
                            Nome = culture.DateTimeFormat.GetMonthName(item.Data.Value.Month)
                        });
                }

                model.Anos.Where(x => x.Valor == item.Data.Value.Year).First()
                    .Meses.Where(x => x.Valor == item.Data.Value.Month).First()
                    .Eventos.Add(item);
            }

            ViewData["from"] = fromDate.ToString("dd/MM/yyyy");
            ViewData["to"] = toDate.ToString("dd/MM/yyyy");

            ViewBag.SearchCliente = (!string.IsNullOrWhiteSpace(cl) ? db.Clientes.FirstOrDefault(x => x.Id.ToString() == cl) : null);
            ViewBag.SearchEspecie = (!string.IsNullOrWhiteSpace(esp) ? db.Especies.FirstOrDefault(x => x.Id.ToString() == esp) : null);

            return View(model);
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