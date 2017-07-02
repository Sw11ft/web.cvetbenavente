using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using System.IO;

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class AnimaisController : Controller
    {
        private readonly ApplicationDbContext db;

        public AnimaisController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Animais
        public IActionResult Index(string esp = null    /*Espécie*/,
                                   int type = 1         /*Tipo (Ambos, Ativo, Inativo)*/,
                                   int col = 1          /*Coluna a Ordenar*/,
                                   string ord = "asc"   /*Ordem (Ascendente, Descendente)*/,
                                   string q = null      /*Pesquisa*/,
                                   string cl = null     /*Cliente*/,
                                   int r = 30           /*Resultados por Página*/,
                                   int p = 1            /*Página*/
                                  )
        {
            ViewData["esp"] = esp;
            ViewData["type"] = type;
            ViewData["col"] = col;
            ViewData["ord"] = ord;
            ViewData["q"] = (q ?? "").Trim();
            ViewData["cl"] = cl;
            ViewBag.SearchCliente = (!string.IsNullOrWhiteSpace(cl) ? db.Clientes.FirstOrDefault(x => x.Id.ToString() == cl) : null);
            ViewBag.SearchEspecie = (!string.IsNullOrWhiteSpace(esp) ? db.Especies.FirstOrDefault(x => x.Id.ToString() == esp) : null);

            var query = db.Animais.Include(a => a.Cliente).Include(a => a.Especie).Where(x => x.Removido == false).AsQueryable();

            /*Espécie*/
            if (!string.IsNullOrWhiteSpace(esp))
            {
                try
                {
                    Guid parsedEsp = Guid.Parse(esp);

                    query = query.Where(x => x.Especie.Id == parsedEsp);
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("The string to be parsed is null.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bad format: {0}", esp);
                }
            }

            /*Tipo*/
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    query = query.Where(x => x.Cliente.Active == true);
                    break;
                case 2:
                    query = query.Where(x => x.Cliente.Active == false);
                    break;
                default:
                    break;
            }

            /*Coluna a Ordenar*/
            switch (col)
            {
                case 0:
                    break;
                case 1: //Nome do Animal
                    switch (ord)
                    {
                        case "asc":
                            query = query.OrderBy(x => x.Nome);
                            break;
                        case "desc":
                            query = query.OrderByDescending(x => x.Nome);
                            break;
                        default:
                            break;
                    }
                    break;
                case 2: //Espécie
                    switch (ord)
                    {
                        case "asc":
                            query = query.OrderBy(x => x.Especie.Nome);
                            break;
                        case "desc":
                            query = query.OrderByDescending(x => x.Especie.Nome);
                            break;
                        default:
                            break;
                    }
                    break;
                case 3: //Nome do Cliente
                    switch (ord)
                    {
                        case "asc":
                            query = query.OrderBy(x => x.Cliente.Nome);
                            break;
                        case "desc":
                            query = query.OrderByDescending(x => x.Cliente.Nome);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            /*Pesquisa*/
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => x.Nome.Contains(q.Trim()));
            }

            /*Cliente*/
            if (!string.IsNullOrWhiteSpace(cl))
            {
                try
                {
                    Guid parsedCl = Guid.Parse(cl);

                    query = query.Where(x => x.Cliente.Id == parsedCl);
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("The string to be parsed is null.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bad format: {0}", cl);
                }
            }

            /*Resultados*/
            r = (r <= 0) ? 1 : r;

            /*Página*/
            p = (p <= 0) ? 1 : p;

            /*Paginação*/
            var maxres = query.Count();

            var maxpages = Math.Ceiling((decimal)maxres / r);


            ViewData["maxres"] = maxres;
            ViewData["maxpages"] = maxpages;

            ViewData["page"] = p;
            ViewData["res"] = r;

            query = query.Skip((r * p) - r).Take(r);

            List<Animal> queryList = query.ToList();

            /*
            foreach (var item in queryList)
            {
                item.Cliente = db.Clientes.FirstOrDefault(x => x.Id == item.IdCliente);
                item.Especie = db.Especies.FirstOrDefault(x => x.Id == item.IdEspecie);
            }
            */
            return View(queryList);
        }

        // GET: Animais/Details/5
        public async Task<IActionResult> Detalhes(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await db.Animais
                .Include(a => a.Cliente)
                .Include(a => a.Especie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null || animal.Removido)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animais/Create
        public IActionResult Criar(string esp = null, string cl = null)
        {
            ViewBag.SearchCliente = (!string.IsNullOrWhiteSpace(cl) ? db.Clientes.FirstOrDefault(x => x.Id.ToString() == cl) : null);
            ViewBag.SearchEspecie = (!string.IsNullOrWhiteSpace(esp) ? db.Especies.FirstOrDefault(x => x.Id.ToString() == esp) : null);

            return View();
        }

        // POST: Animais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Animal animal)
        {
            if (ModelState.IsValid)
            {
                animal.Id = Guid.NewGuid();
                animal.DataCriacao = DateTime.UtcNow;
                db.Add(animal);

                await db.SaveChangesAsync();
                return RedirectToAction("Detalhes", "Animais", new { id = animal.Id, nid = 5, nt = "s" });
            }
            return View(animal);
        }

        // GET: Animais/Edit/5
        public async Task<IActionResult> Editar(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await db.Animais.SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.SearchCliente = db.Clientes.Where(x => x.Id == animal.IdCliente).FirstOrDefault();
            ViewBag.SearchEspecie = db.Especies.Where(x => x.Id == animal.IdEspecie).FirstOrDefault();

            return View(animal);
        }

        // POST: Animais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, [Bind("Id,IdCliente,Nome,Genero,IdEspecie")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(animal);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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
            ViewData["IdCliente"] = new SelectList(db.Clientes, "Id", "Nome", animal.IdCliente);
            ViewData["IdEspecie"] = new SelectList(db.Especies, "Id", "Nome", animal.IdEspecie);
            return View(animal);
        }

        // GET: Animais/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await db.Animais
                .Include(a => a.Cliente)
                .Include(a => a.Especie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var animal = await db.Animais.SingleOrDefaultAsync(m => m.Id == id);
            db.Animais.Remove(animal);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult ExportToExcel()
        {
            var fileDownloadName = "Animais." + DateTime.UtcNow.ToString("dd-MM-yyyy.HHmm") + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Página 1");
            worksheet.Cells["A2"].Value = "Nome";
            worksheet.Cells["B2"].Value = "Género";
            worksheet.Cells["C2"].Value = "Espécie";
            worksheet.Cells["D2"].Value = "Observações";
            worksheet.Cells["E2"].Value = "Cliente";
            worksheet.Cells["F2"].Value = "Contacto";
            worksheet.Cells["G2"].Value = "Morada";

            worksheet.Cells["A1:G1"].Merge = true;
            worksheet.Cells["A1:G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            worksheet.Cells["A1:G1"].Value = "CVETBENAVENTE - Animais";

            worksheet.Row(2).Style.Font.Bold = true;

            var data = db.Animais.Where(x => x.Cliente.Active && x.Removido == false).Include(x => x.Cliente).Include(x => x.Especie).OrderBy(x => x.Nome).ToList();

            var i = 3;
            foreach (var item in data)
            {
                worksheet.Cells["A" + i].Value = item.Nome;
                worksheet.Cells["B" + i].Value = item.Genero;
                worksheet.Cells["C" + i].Value = item.Especie.Nome;
                worksheet.Cells["D" + i].Value = item.Observacoes;
                worksheet.Cells["E" + i].Value = item.Cliente.Nome;
                worksheet.Cells["F" + i].Value = item.Cliente.Contacto;
                worksheet.Cells["G" + i].Value = item.Cliente.Morada + ", " + item.Cliente.CodPostal + " " + item.Cliente.Localidade;
                i++;
            }

            worksheet.Cells["A" + (i + 2) + ":G" + (i + 2)].Merge = true;
            worksheet.Cells["A" + (i + 2) + ":G" + (i + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            worksheet.Cells["A" + (i + 2) + ":G" + (i + 2)].Value = DateTime.UtcNow + " UTC - Processado por computador";

            worksheet.Column(1).Width = 15;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 40;
            worksheet.Column(5).Width = 25;
            worksheet.Column(6).Width = 15;
            worksheet.Column(7).Width = 60;


            var fileStream = new MemoryStream();
            package.SaveAs(fileStream);
            fileStream.Position = 0;

            var fileStreamResult = new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownloadName
            };
            return fileStreamResult;
        }

        private bool AnimalExists(Guid id, int clActive = 1, int removido = 0)
        {
            var animais = db.Animais.Include(x => x.Cliente).AsQueryable();

            switch (clActive)
            {
                case 0:
                    animais = animais.Where(x => !x.Cliente.Active);
                    break;
                case 1:
                    animais = animais.Where(x => x.Cliente.Active);
                    break;
                default:
                    break;
            }

            switch (removido)
            {
                case 0:
                    animais = animais.Where(x => !x.Removido);
                    break;
                case 1:
                    animais = animais.Where(x => x.Removido);
                    break;
                default:
                    break;
            }

            return animais.Any();
        }

        public IActionResult GetAnimais(int ativo = 1,
                                        int removido = 0,
                                        string q = null /*query*/,
                                        string cl = null /*cliente*/,
                                        bool showWithoutCl = true /*mostrar mesmo que cl seja nulo*/,
                                        int page = 1 /*paginação*/,
                                        int mr = 15 /*max results*/)
        {
            if (!showWithoutCl && !string.IsNullOrWhiteSpace(cl))
            {
                var animais = db.Animais.Include(x => x.Cliente).Include(x => x.Especie).Where(x => x.Removido == false).AsQueryable();

                switch (ativo)
                {
                    case 0:
                        animais = animais.Where(x => !x.Cliente.Active);
                        break;
                    case 1:
                        animais = animais.Where(x => x.Cliente.Active);
                        break;
                    default:
                        break;
                }

                switch (removido)
                {
                    case 0:
                        animais = animais.Where(x => !x.Removido);
                        break;
                    case 1:
                        animais = animais.Where(x => x.Removido);
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrWhiteSpace(q))
                {
                    animais = animais.Where(x => x.Cliente.Nome.Contains(q)
                           || x.Cliente.Contacto.Contains(q)
                           || x.Cliente.Morada.Contains(q)
                           || x.Cliente.CodPostal.Contains(q)
                           || x.Cliente.Localidade.Contains(q)
                           || x.Nome.Contains(q)
                           || x.Especie.Nome.Contains(q)
                           || x.Especie.NomeF.Contains(q)
                           || x.Observacoes.Contains(q));
                }

                animais = animais.Where(x => x.Cliente.Id.ToString() == cl);

                var totalAnimais = animais.Count();
                var listAnimais = animais.Select(x => new { id = x.Id,
                                                            text = x.Nome,
                                                            x.Genero,
                                                            espNome = x.Especie.Nome,
                                                            espNomeF = x.Especie.NomeF,
                                                            espImg = x.Especie.Imagem,
                                                            clNome = x.Cliente.Nome,
                                                            clContacto = x.Cliente.Contacto
                                                           }
                                                ).OrderBy(x => x.text).Skip(mr * (page - 1)).Take(mr).ToList();

                return Json(new { total_items = totalAnimais, items = listAnimais });
            }
            return Json(new { total_items = 0, items = new List<object>() });
        }

        public bool AnimalBelongsToCliente(string idAnimal, string idCliente)
        {
            return db.Animais.Any(x => x.Id.ToString() == idAnimal && x.Cliente.Id.ToString() == idCliente);
        }
    }
}
