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
        public IActionResult Index()
        {
            ClienteServices service = new ClienteServices(db);

            return View(service.GetClientes(Enums.TipoAtivo.Ativo, Enums.OrderClientes.Nome, Enums.OrderDirection.Asc));
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

        public bool ClienteExists(Guid id)
        {
            return db.Clientes.Any(e => e.Id == id);
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
        public bool ClienteExistsByStringId(string IdCliente)
        {
            try
            {
                Guid xIdCliente = Guid.Parse(IdCliente);
                return db.Clientes.Any(x => x.Id == xIdCliente);
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
