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

            var query = db.Animais.Include(a => a.Cliente).Include(a => a.Especie).AsQueryable();

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

            /*Espécie*/
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
                    Console.WriteLine("Bad format: {0}", esp);
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
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animais/Create
        public IActionResult Criar()
        {
            ViewData["IdCliente"] = new SelectList(db.Clientes, "Id", "Nome");
            ViewData["IdEspecie"] = new SelectList(db.Especies, "Id", "Nome");
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
            return View(animal);
        }

        // POST: Animais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
           /* var data = db.Animais
                           .Where(x => x.Cliente.Active)
                           .Select(x => new {
                               Nome = x.Nome,
                               Genero = x.Genero,
                               Especie = x.Especie.Nome,
                               Observacoes = x.Observacoes,
                               Cliente = x.Cliente.Nome,
                               Contacto = x.Cliente.Contacto,
                               Morada = x.Cliente.Morada + ", " + x.Cliente.CodPostal + " " + x.Cliente.Localidade,
                               Observacoes_Cliente = x.Cliente.Observacoes
                           }); */

            var fileDownloadName = "Animais.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Página 1");
            worksheet.Cells["A1"].Value = "Teste 1";
            worksheet.Cells["A1"].Style.Font.Bold = true;

            var fileStream = new MemoryStream();
            package.SaveAs(fileStream);
            fileStream.Position = 0;

            var fileStreamResult = new FileStreamResult(fileStream, contentType);
            fileStreamResult.FileDownloadName = fileDownloadName;

            return fileStreamResult;
        }


        private bool AnimalExists(Guid id)
        {
            return db.Animais.Any(e => e.Id == id);
        }
    }
}
