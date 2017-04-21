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
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IndexTableData
        public IActionResult IndexTableData(string field = "nome", string order = "asc", string query = null)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ClienteServices service = new ClienteServices(_context);

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

                var clientes = service.GetClientes(Enums.TipoAtivo.Ativo, enumField, enumOrder, query);

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
            ClienteServices service = new ClienteServices(_context);

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
            ClienteServices service = new ClienteServices(_context);

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
            ClienteServices service = new ClienteServices(_context);

            return View(service.GetClientes(Enums.TipoAtivo.Ativo, Enums.OrderClientes.Nome, Enums.OrderDirection.Asc));
        }

        // GET: Clientes/Details/5
        public IActionResult Detalhes(Guid? id)
        {
            ClienteServices service = new ClienteServices(_context);

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
            ClienteServices service = new ClienteServices(_context);

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

            var cliente = await _context.Clientes.SingleOrDefaultAsync(m => m.Id == id);
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
                ClienteServices service = new ClienteServices(_context);
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

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
