using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using web.cvetbenavente.Models.EspeciesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class EspeciesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment _env;
        public EspeciesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            db = context;
            _env = env;
        }

        // GET: Especies
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            List<Especie> Especies = db.Especies.Where(x => x.Active == true).ToList();

            foreach (var item in Especies)
            {
                IndexViewModel.Especie especie = new IndexViewModel.Especie();

                especie.Id = item.Id;
                especie.Nome = item.Nome;
                especie.NrAnimais = db.Animais.Where(x => x.IdEspecie == item.Id && x.Cliente.Active == true).Count();

                model.Especies.Add(especie);
            }

            model.NrEspecies = Especies.Count();

            return View(model);
        }

        // GET: Especies/Details/5
        public async Task<IActionResult> Detalhes(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await db.Especies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            DetalhesViewModel model = new DetalhesViewModel();

            model.Id = especie.Id;
            model.Nome = especie.Nome;
            model.Imagem = especie.Imagem;
            model.DataCriacao = especie.DataCriacao;
            model.NrAnimais = db.Animais.Where(x => x.IdEspecie == especie.Id).Count();

            return View(model);
        }

        // GET: Especies/Create
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Especies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar(CriarViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Especies.Any(x => x.Nome.ToLowerInvariant() == model.Especie.Nome.ToLowerInvariant()) == false)
                {
                    Especie especie = model.Especie;

                    especie.DataCriacao = DateTime.UtcNow;
                    especie.Id = Guid.NewGuid();
                    especie.Active = true;

                    db.Especies.Add(especie);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { nid = 4, nt = "s" });
                }
                else
                {
                    return RedirectToAction("Criar", new { nid = 1, nt = "e" });
                }
            }
            return View(model);
        }

        // GET: Especies/Edit/5
        public async Task<IActionResult> Editar(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await db.Especies.SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }
            return View(especie);
        }

        // POST: Especies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, [Bind("Id,Nome,Active")] Especie especie)
        {
            if (id != especie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(especie);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecieExists(especie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(especie);
        }

        // GET: Especies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await db.Especies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // POST: Especies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var especie = await db.Especies.SingleOrDefaultAsync(m => m.Id == id);
            db.Especies.Remove(especie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EspecieExists(Guid id)
        {
            return db.Especies.Any(e => e.Id == id);
        }
        private bool EspecieExists(string nome)
        {
            return db.Especies.Any(x => x.Nome.ToLowerInvariant() == nome.ToLowerInvariant());
        }

        [HttpGet]
        public ActionResult EspecieNameValid([Bind(Prefix = "Especie.Nome")] string Nome)
        {
                return EspecieExists(Nome) ? Json(false) : Json(true);
        }
    }
}
