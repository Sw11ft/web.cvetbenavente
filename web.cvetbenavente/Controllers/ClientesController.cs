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
using web.cvetbenavente.Models.ClientesViewModels;
using web.cvetbenavente.Services;
using OfficeOpenXml;
using System.IO;

namespace web.cvetbenavente.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ClientesController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: IndexTableData
        public IActionResult IndexTableData(string field = "nome", string order = "asc", string query = null, int estado = 1)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ClienteServices service = new ClienteServices(db);

                field = field.ToLower();
                order = order.ToLower();

                if (query != null)
                    if (query.Trim() == "")
                        query = null;


                //Campo
                Enums.OrderClientes enumField = Enums.OrderClientes.Nome;
                switch (field)
                {
                    case "nome":
                        enumField = Enums.OrderClientes.Nome;
                        break;
                    case "morada":
                        enumField = Enums.OrderClientes.Morada;
                        break;
                    default:
                        enumField = Enums.OrderClientes.Nome;
                        break;
                }

                //Ordem
                Enums.OrderDirection enumOrder = Enums.OrderDirection.Asc;
                switch (order)
                {
                    case "asc":
                        enumOrder = Enums.OrderDirection.Asc;
                        break;
                    case "desc":
                        enumOrder = Enums.OrderDirection.Desc;
                        break;
                    default:
                        enumOrder = Enums.OrderDirection.Asc;
                        break;
                }

                //Estado
                Enums.TipoAtivo tipoAtivo = Enums.TipoAtivo.Ativo;
                switch (estado)
                {
                    case 0:
                        tipoAtivo = Enums.TipoAtivo.Ambos;
                        break;
                    case 1:
                        tipoAtivo = Enums.TipoAtivo.Ativo;
                        break;
                    case 2:
                        tipoAtivo = Enums.TipoAtivo.Inativo;
                        break;
                    default:
                        tipoAtivo = Enums.TipoAtivo.Ativo;
                        break;
                }
                var clientes = service.GetClientes(tipoAtivo, enumField, enumOrder, query);

                return PartialView("_IndexTablePartial", clientes);
            }
            else
            {
                return StatusCode(403);
            }
        }

        // POST: Desativar Cliente
        [HttpPost]
        public bool DisableCliente(string Id)
        {
            ClienteServices service = new ClienteServices(db);

            bool validGuid = true;
            Guid Guid;

            try
            {
                Guid = new Guid(Id);
            }
            catch
            {
                validGuid = false;
            }

            if (validGuid)
            {
                Cliente cliente = service.GetClienteById(Guid);

                if (cliente == null)
                {
                    return false;
                }
                else
                {
                    service.DisableCliente(Guid);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        // POST: Ativar Cliente
        [HttpPost]
        public bool EnableCliente(string Id)
        {
            ClienteServices service = new ClienteServices(db);

            bool validGuid = true;
            Guid Guid;

            try
            {
                Guid = new Guid(Id);
            }
            catch
            {
                validGuid = false;
            }

            if (validGuid)
            {
                Cliente cliente = service.GetClienteById(Guid);

                if (cliente == null)
                {
                    return false;
                }
                else
                {
                    service.EnableCliente(Guid);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        // GET: Clientes
        public IActionResult Index(int type = 1         /*Tipo (Ambos, Ativo, Inativo)*/,
                                   int col = 1          /*Coluna a Ordenar*/,
                                   string ord = "asc"   /*Ordem (Ascendente, Descendente)*/,
                                   string q = null      /*Pesquisa*/,
                                   int r = 30           /*Resultados por Página*/,
                                   int p = 1            /*Página*/
                                  )
        {
            ViewData["type"] = type;
            ViewData["col"] = col;
            ViewData["ord"] = ord;
            ViewData["q"] = (q ?? "").Trim();

            var query = db.Clientes.AsQueryable();

            /*Tipo*/
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    query = query.Where(x => x.Active == true);
                    break;
                case 2:
                    query = query.Where(x => x.Active == false);
                    break;
                default:
                    break;
            }

            /*Coluna a Ordenar*/
            switch (col)
            {
                case 1: //Nome
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
            }

            /*Pesquisa*/
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(x => x.Nome.Contains(q) || x.CodPostal.Contains(q) || x.Morada.Contains(q));
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

            List<Cliente> queryList = query.ToList();

            return View(queryList);
        }

        // GET: Clientes/Details/5
        public IActionResult Detalhes(Guid? id)
        {
            ClienteServices service = new ClienteServices(db);

            if (id == null)
            {
                return NotFound();
            }

            Guid xId = id ?? default(Guid);

            Cliente cliente = service.GetClienteById(xId);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Criar()
        {
            CriarViewModel model = new CriarViewModel();
            return View(model);
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(CriarViewModel model)
        {
            ClienteServices service = new ClienteServices(db);

            var clienteModel = model.Cliente;
            if (ModelState.IsValid)
            {
                Cliente cliente = new Cliente
                {
                    Id = Guid.NewGuid(),

                    Nome = clienteModel.Nome,
                    Contacto = clienteModel.Contacto,

                    Morada = clienteModel.Morada,
                    CodPostal = clienteModel.CodPostal,
                    Localidade = clienteModel.Localidade,

                    Observacoes = clienteModel.Observacoes,
                    Active = true,

                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = null
                };

                service.CreateCliente(cliente);

                return RedirectToAction("Index", "Clientes", new { nid = 1, nt = "s"});
            }
            else
            {
                return View(model);
            }
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Editar(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await db.Clientes.SingleOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                ClienteServices service = new ClienteServices(db);
                if (service.Exists(cliente.Id))
                {
                    if (service.IsActive(cliente.Id))
                    {
                        cliente.Active = true;
                        service.EditCliente(cliente);
                        return RedirectToAction("Detalhes", "Clientes", new { id = cliente.Id, nid = 10, nt = "s" });
                    }
                    else
                    {
                        return RedirectToAction("Detalhes", "Clientes", new { id = cliente.Id, nid = 12, nt = "e" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Clientes", new { nid = 11, nt = "e" });
                }
            }
            return View(cliente);
        }

        public IActionResult ExportToExcel()
        {
            var fileDownloadName = "Clientes." + DateTime.UtcNow.ToString("dd-MM-yyyy.hhmm") + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Página 1");
            worksheet.Cells["A2"].Value = "Nome";
            worksheet.Cells["B2"].Value = "Contacto";
            worksheet.Cells["C2"].Value = "Morada";
            worksheet.Cells["D2"].Value = "Observações";
            worksheet.Cells["E2"].Value = "Data de Criação";

            worksheet.Cells["A1:E1"].Merge = true;
            worksheet.Cells["A1:E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            worksheet.Cells["A1:E1"].Value = "CVETBENAVENTE - Clientes";

            worksheet.Row(2).Style.Font.Bold = true;

            var data = db.Clientes.Where(x => x.Active).OrderBy(x => x.Nome).OrderBy(x => x.DataCriacao);

            var i = 3;
            foreach (var item in data)
            {
                worksheet.Cells["A" + i].Value = item.Nome;
                worksheet.Cells["B" + i].Value = item.Contacto;
                worksheet.Cells["C" + i].Value = item.Morada + ", " + item.CodPostal + " " + item.Localidade;
                worksheet.Cells["D" + i].Value = item.Observacoes;
                worksheet.Cells["E" + i].Value = item.DataCriacao.ToString("dd/MM/yyyy hh:mm");

                i++;
            }

            worksheet.Cells["A" + (i + 2) + ":E" + (i + 2)].Merge = true;
            worksheet.Cells["A" + (i + 2) + ":E" + (i + 2)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
            worksheet.Cells["A" + (i + 2) + ":E" + (i + 2)].Value = DateTime.UtcNow + " UTC - Processado por computador";

            worksheet.Column(1).Width = 25;
            worksheet.Column(2).Width = 15;
            worksheet.Column(3).Width = 70;
            worksheet.Column(4).Width = 40;
            worksheet.Column(5).Width = 25;


            var fileStream = new MemoryStream();
            package.SaveAs(fileStream);
            fileStream.Position = 0;

            var fileStreamResult = new FileStreamResult(fileStream, contentType);
            fileStreamResult.FileDownloadName = fileDownloadName;

            return fileStreamResult;
        }

        public bool ClienteExists(Guid id, int active = 1)
        {
            var cl = db.Clientes.AsQueryable();

            switch (active)
            {
                case 0:
                    cl = cl.Where(x => !x.Active);
                    break;
                case 1:
                    cl = cl.Where(x => x.Active);
                    break;
                default:
                    break;
            }

            return cl.Any();
        }
        public bool ClienteExists(string id) {
            try
            {
                Guid parsedId = Guid.Parse(id);
                return ClienteExists(parsedId);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        public bool ClienteExistsByStringId(string IdCliente, int active = 1)
        {
            try
            {
                Guid xIdCliente = Guid.Parse(IdCliente);
                var cl = db.Clientes.AsQueryable();

                switch (active)
                {
                    case 0:
                        cl = cl.Where(x => !x.Active);
                        break;
                    case 1:
                        cl = cl.Where(x => x.Active);
                        break;
                    default:
                        break;
                }

                return cl.Any();
            }
            catch
            {
                return false;
            }
        }

        public IActionResult GetClientes(int ativo = 1, string q = null /*query*/, int page = 1 /*paginação*/, int mr = 15)
        {
            var clientes = db.Clientes.AsQueryable();

            switch (ativo)
            {
                case 0:
                    clientes = clientes.Where(x => x.Active == false);
                    break;
                case 1:
                    clientes = clientes.Where(x => x.Active == true);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                clientes = clientes.Where(x => x.Nome.Contains(q) || x.Contacto.Contains(q) || x.Morada.Contains(q) || x.CodPostal.Contains(q) || x.Localidade.Contains(q));
            }

            var totalClientes = clientes.Count();
            var listClientes = clientes.Select(x => new { id = x.Id, text = x.Nome, x.Contacto, x.Morada, x.Localidade, x.CodPostal }).OrderBy(x => x.text).Skip(mr * (page - 1)).Take(mr).ToList();

            return Json(new { total_items = totalClientes, items = listClientes });

        }
    }
}
