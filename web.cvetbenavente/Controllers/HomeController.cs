using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web.cvetbenavente.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Models.HomeViewModels.IndexViewModel model = new Models.HomeViewModels.IndexViewModel();

            CultureInfo ptCulture = new CultureInfo("pt-PT");

            #region mainGraph
            for (int i = 5; i >= 0; i--)
            {
                var startOfMonth = new DateTime(DateTime.UtcNow.AddMonths(-i).Year, DateTime.UtcNow.AddMonths(-i).Month, 1);
                var endOfMonth = new DateTime(startOfMonth.Year, startOfMonth.Month, DateTime.DaysInMonth(startOfMonth.Year, startOfMonth.Month), 23, 59, 59);


                var cl = db.Clientes.Where(x => x.Active)
                                      .Where(x => x.DataCriacao >= startOfMonth)
                                      .Where(x => x.DataCriacao <= endOfMonth).Count();

                var animais = db.Animais.Where(x => x.Cliente.Active && !x.Removido)
                                      .Where(x => x.DataCriacao >= startOfMonth)
                                      .Where(x => x.DataCriacao <= endOfMonth).Count();

                model.MainGraph.Meses.Add(new Models.HomeViewModels.IndexViewModel.MainGraphData.Mes
                {
                    Nome = ptCulture.DateTimeFormat.GetMonthName(startOfMonth.Month),
                    NovosAnimais = animais,
                    NovosClientes = cl
                });
            }
            #endregion

            #region pieData
            var pieData = (from Especies in db.Especies
                        select new
                        {
                            id = Especies.Id,
                            label = Especies.Nome,
                            value = (
                                from Animais in db.Animais
                                where Animais.IdEspecie == Especies.Id
                                    && Especies.Active == true
                                    && Animais.Cliente.Active == true
                                    && Animais.Removido == false
                                select new { Animais }
                            ).Count()
                        }).OrderByDescending(x => x.value).ThenBy(x => x.label);

            foreach (var item in pieData)
            {
                Random rnd = new Random();

                model.TopEspecies.Labels.Add(item.label);
                model.TopEspecies.Valores.Add(item.value);
                model.TopEspecies.RGB.Add(new Tuple<int, int, int>(rnd.Next(1, 256), rnd.Next(1, 256), rnd.Next(1, 256)));

                model.NrAnimais += item.value;
            }
            #endregion

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
