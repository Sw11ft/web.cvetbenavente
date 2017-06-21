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
            //var modelosList = new List<SelectListItem>();
            //modelosList.Add(new SelectListItem
            //{
            //    Text =
            //})
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
    }
}