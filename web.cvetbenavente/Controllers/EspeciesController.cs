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
using System.Drawing;
using System.IO;
using web.cvetbenavente.Services;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

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

            List<Especie> Especies = db.Especies.Where(x => x.Active == true).OrderBy(x => x.Nome).ToList();

            foreach (var item in Especies)
            {
                IndexViewModel.Especie especie = new IndexViewModel.Especie()
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    NomeF = item.NomeF,
                    Imagem = item.Imagem,
                    NrAnimais = db.Animais.Where(x => x.IdEspecie == item.Id && x.Cliente.Active == true).Count()
                };

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

            DetalhesViewModel model = new DetalhesViewModel()
            {
                Id = especie.Id,
                Nome = especie.Nome,
                NomeF = especie.NomeF,
                Imagem = especie.Imagem,
                DataCriacao = especie.DataCriacao,
                NrAnimais = db.Animais.Where(x => x.IdEspecie == especie.Id).Count()
            };
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
        public async Task<ActionResult> Criar(CriarViewModel model, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (db.Especies.Any(x => x.Nome.ToLowerInvariant() == model.Especie.Nome.ToLowerInvariant()) == false)
                {

                    Especie especie = model.Especie;
                    var a = model.Imagem;
                    especie.DataCriacao = DateTime.UtcNow;
                    especie.Id = Guid.NewGuid();
                    especie.Active = true;

                    if (model.Imagem != null)
                    {
                        var img = model.Imagem;
                        Image imgStream = Image.FromStream(img.OpenReadStream());

                        Stream ms = new MemoryStream(imgStream.Resize(100, 100).ToByteArray());

                        var uploads = Path.Combine(_env.WebRootPath, "upload\\img\\especies");

                        var fileNameSplit = img.FileName.Split('.');
                        var extension = fileNameSplit[fileNameSplit.Length - 1];
                        var fileName = especie.Id.ToString() + "." + extension;

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await ms.CopyToAsync(fileStream);
                            especie.Imagem = fileName;
                        }
                    }

                    db.Especies.Add(especie);
                    await db.SaveChangesAsync();

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
            EditarViewModel model = new EditarViewModel()
            {
                Especie = especie
            };
            return View(model);
        }

        // POST: Especies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid? id, EditarViewModel model, IList<IFormFile> files)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Guid xid = id ?? default(Guid);

                if (db.Especies.SingleOrDefaultAsync(x => x.Id == xid) == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        Especie especieOriginal = db.Especies.Find(id);

                        Especie especieAlterada = model.Especie;


                        especieAlterada.DataEdicao = DateTime.UtcNow;
                        especieAlterada.DataCriacao = especieOriginal.DataCriacao;
                        especieAlterada.Active = true;
                        especieAlterada.Id = xid;

                        if (model.Imagem != null)
                        {
                            var img = model.Imagem;
                            Image imgStream = Image.FromStream(img.OpenReadStream());

                            Stream ms = new MemoryStream(imgStream.Resize(100, 100).ToByteArray());

                            var uploads = Path.Combine(_env.WebRootPath, "upload\\img\\especies");

                            var fileNameSplit = img.FileName.Split('.');
                            var extension = fileNameSplit[fileNameSplit.Length - 1];
                            var fileName = especieAlterada.Id.ToString() + "." + extension;

                            if (!string.IsNullOrWhiteSpace(especieOriginal.Imagem))
                            {
                                if (System.IO.File.Exists(Path.Combine(uploads, especieOriginal.Imagem))) {
                                    System.IO.File.Delete(Path.Combine(uploads, especieOriginal.Imagem));
                                }
                            }

                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await ms.CopyToAsync(fileStream);
                                especieAlterada.Imagem = fileName;
                            }
                        }
                        else
                        {
                            especieAlterada.Imagem = especieOriginal.Imagem;
                        }

                        db.Entry(especieOriginal).CurrentValues.SetValues(especieAlterada);

                        await db.SaveChangesAsync();

                        return RedirectToAction("Detalhes", new { id = especieOriginal.Id, nt = "s", nid = 40 });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EspecieExists(model.Especie.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return View(model);
            }
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
        private bool EspecieExistsExcept(string nome, Guid id)
        {
            return db.Especies.Any(x => x.Nome.ToLowerInvariant() == nome.ToLowerInvariant() && x.Id != id);
        }
        private bool EspecieExistsExcept(string nome, string id)
        {
            try
            {
                Guid xid = Guid.Parse(id);
                return db.Especies.Any(x => x.Nome.ToLowerInvariant() == nome.ToLowerInvariant() && x.Id != xid);
            }
            catch
            {
                return true;
            }
        }
        public bool EspecieExistsByStringId(string IdEspecie)
        {
            try
            {
                Guid xIdEspecie = Guid.Parse(IdEspecie);
                return db.Especies.Any(x => x.Id == xIdEspecie);
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public ActionResult EspecieNameValid([Bind(Prefix = "Especie.Nome")] string Nome, [Bind(Prefix = "Especie.Id")] string Id = null)
        {
                return (string.IsNullOrWhiteSpace(Id)) ? (EspecieExists(Nome) ? Json(false) : Json(true))
                                                       : (EspecieExistsExcept(Nome, Id) ? Json(false) : Json(true));
        }

        [HttpPost]
        public bool ApagarEspecie (string Id)
        {
            Especie especie = db.Especies.Where(x => x.Id.ToString() == Id).SingleOrDefault();

            if (especie == null)
            {
                return false;
            }
            else
            {
                if (db.Animais.Where(x => x.IdEspecie == especie.Id).Any())
                {
                    try
                    {
                        especie.Active = false;
                        db.Especies.Update(especie);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
                else
                { 
                    try
                    {
                        db.Especies.Remove(especie);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        [HttpGet]
        public ActionResult GetEspecies(string q /*query*/, int mr = 10 /*maxresults*/)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                var esp =
                        (
                            from Especies in db.Especies
                            orderby (
                                from Animais in db.Animais
                                where Animais.IdEspecie == Especies.Id && Animais.Removido == false
                                select new { Animais }
                            ).Count() descending, Especies.Nome
                            select new
                            {
                                id = Especies.Id,
                                text = Especies.Nome,
                                nrAnimais = (
                                    from Animais in db.Animais
                                    where Animais.IdEspecie == Especies.Id && Especies.Active == true
                                    select new { Animais }
                                ).Count()
                            }
                        ).Take(mr).ToList();

                return Json(esp);
            } else
            {
                var esp =
                        (
                            from Especies in db.Especies
                            where Especies.Nome.Contains(q)
                            orderby (
                                from Animais in db.Animais
                                where Animais.IdEspecie == Especies.Id && Animais.Removido == false
                                select new { Animais }
                            ).Count() descending, Especies.Nome
                            select new {
                                id = Especies.Id,
                                text = Especies.Nome,
                                nrAnimais = (
                                    from Animais in db.Animais
                                    where Animais.IdEspecie == Especies.Id && Especies.Active == true
                                    select new { Animais }
                                ).Count()
                            }
                        ).Take(mr).ToList();

                return Json(esp);
            }
        }
    }
}
