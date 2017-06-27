using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web.cvetbenavente.Data;
using Microsoft.EntityFrameworkCore;

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

            var data = (from Especies in db.Especies
                        select new
                        {
                            id = Especies.Id,
                            label = Especies.Nome,
                            value = (
                                from Animais in db.Animais
                                where Animais.IdEspecie == Especies.Id && Especies.Active == true
                                select new { Animais }
                            ).Count()
                        }).OrderByDescending(x => x.value).ThenBy(x => x.label);

            foreach (var item in data)
            {
                Random rnd = new Random();

                model.TopEspecies.Labels.Add(item.label);
                model.TopEspecies.Valores.Add(item.value);
                model.TopEspecies.RGB.Add(new Tuple<int, int, int>(rnd.Next(1, 256), rnd.Next(1, 256), rnd.Next(1, 256)));
            }
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
