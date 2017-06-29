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
                        }).OrderByDescending(x => x.value).ThenBy(x => x.label).Take(8);

            List<Tuple<int, int, int>> rgb = new List<Tuple<int, int, int>>();

            rgb.Add(new Tuple<int, int, int>(244, 67, 54)); //
            rgb.Add(new Tuple<int, int, int>(255, 106, 60)); //
            rgb.Add(new Tuple<int, int, int>(255, 162, 26)); //
            rgb.Add(new Tuple<int, int, int>(255, 199, 33)); //
            rgb.Add(new Tuple<int, int, int>(76, 175, 80)); //
            rgb.Add(new Tuple<int, int, int>(0, 150, 136)); //
            rgb.Add(new Tuple<int, int, int>(0, 188, 212)); //
            rgb.Add(new Tuple<int, int, int>(3, 169, 244)); //

            var index = 0;
            foreach (var item in pieData)
            {

                model.TopEspecies.Labels.Add(item.label);
                model.TopEspecies.Valores.Add(item.value);
                model.TopEspecies.RGB.Add(new Tuple<int, int, int>(rgb[index].Item1, rgb[index].Item2, rgb[index].Item3));

                model.NrAnimais += item.value;

                index++;
            }
            #endregion

            model.NrClientes = db.Clientes.Where(x => x.Active).Count();

            model.AnimaisPorCliente = (float)model.NrAnimais / (float)model.NrClientes;

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
