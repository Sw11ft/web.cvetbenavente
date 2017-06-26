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
        public IActionResult Index(DateTime? from = null,
                                   DateTime? to = null,
                                   string cl = null,
                                   string esp = null,
                                   string animal = null)
        {
            from = from ?? DateTime.UtcNow;

            Models.EventosViewModels.IndexViewModel model = new Models.EventosViewModels.IndexViewModel();

            var eventos = db.Eventos.Where(x => x.Data != null).Include(x => x.Cliente).Include(x => x.Animal).AsQueryable();

            eventos = eventos.Where(x => x.Data > from);

            if (to != null)
            {
                eventos = eventos.Where(x => x.Data <= to);
            }

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

            foreach (var item in eventos.ToList())
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
                            Nome = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Data.Value.Month)
                        });
                }

                model.Anos.Where(x => x.Valor == item.Data.Value.Year).First()
                    .Meses.Where(x => x.Valor == item.Data.Value.Month).First()
                    .Eventos.Add(item);
            }

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