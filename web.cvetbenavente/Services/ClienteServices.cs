using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using static web.cvetbenavente.Models.Enums;
using static web.cvetbenavente.Services.Helpers;

namespace web.cvetbenavente.Services
{
    public class ClienteServices
    {
        private readonly ApplicationDbContext db;

        public ClienteServices(ApplicationDbContext context)
        {
            db = context;
        }

        //Retorna uma lista de clientes
        public List<Cliente> GetClientes(TipoAtivo? TipoAtivo = TipoAtivo.Ambos,
                                         OrderClientes? OrderBy = OrderClientes.NoOrder,
                                         OrderDirection? OrderDir = OrderDirection.Asc,
                                         string searchText = null)
        {
            var query = db.Clientes.AsQueryable();

            //Converte TipoAtivo? para TipoAtivo
            //Caso TipoAtivo? seja nulo, o valor atribuido
            //é o valor default (ou seja, no caso dos enums, o primeiro valor)
            var xTipoAtivo = TipoAtivo ?? default(TipoAtivo);
            var xOrderBy = OrderBy ?? default(OrderClientes);
            var xOrderDir = OrderDir ?? default(OrderDirection);

            switch (xTipoAtivo)
            {
                case Enums.TipoAtivo.Ambos:
                    break;
                case Enums.TipoAtivo.Ativo:
                    query = query.Where(x => x.Active == true);
                    break;
                case Enums.TipoAtivo.Inativo:
                    query = query.Where(x => x.Active == false);
                    break;
                default:
                    break;
            }

            switch (xOrderBy)
            {
                case OrderClientes.NoOrder:
                    break;
                case OrderClientes.Nome:
                    query = (xOrderDir == OrderDirection.Asc) ? query.OrderBy(x => x.Nome)
                                                              : query.OrderByDescending(x => x.Nome);
                    break;
                case OrderClientes.Contacto:
                    query = (xOrderDir == OrderDirection.Asc) ? query.OrderBy(x => x.Contacto)
                                                              : query.OrderByDescending(x => x.Contacto);
                    break;
                case OrderClientes.Morada:
                    query = (xOrderDir == OrderDirection.Asc) ? query.OrderBy(x => x.Morada)
                                                              : query.OrderByDescending(x => x.Morada);
                    break;
                case OrderClientes.Active:
                    query = (xOrderDir == OrderDirection.Asc) ? query.OrderBy(x => x.Active)
                                                              : query.OrderByDescending(x => x.Active);
                    break;
                default:
                    break;
            }

            if (searchText != null)
            {
                searchText = NormalizeString(searchText).ToLower();

                query = query.Where(x => (NormalizeString(x.Nome).ToLower()).Contains(searchText)
                                      || (NormalizeString(x.Morada).ToLower()).Contains(searchText)
                                      || (NormalizeString(x.CodPostal).ToLower()).Contains(searchText)
                                      || (NormalizeString(x.Localidade).ToLower()).Contains(searchText)
                                    );
            }

            return query.ToList();
        }

        //Retorna um cliente com base no Id
        public Cliente GetClienteById(Guid Guid)
        {
            return db.Clientes.Find(Guid);
        }

        //Adiciona um cliente
        public void CreateCliente(Cliente cliente)
        {
            db.Clientes.Add(cliente);
            db.SaveChanges();
        }

        //Edita um cliente
        public void EditCliente(Cliente cliente)
        {
            Cliente clienteOriginal = db.Clientes.Find(cliente.Id);
            db.Entry(clienteOriginal).CurrentValues.SetValues(cliente);

            db.SaveChanges();
        }

        //"Remove" (desativa) um cliente
        public void DisableCliente(Guid id)
        {
            Cliente cliente = db.Clientes.Find(id);
            cliente.Active = false;

            db.SaveChanges();
        }

        //"Ativa" um cliente
        public void EnableCliente(Guid id)
        {
            Cliente cliente = db.Clientes.Find(id);
            cliente.Active = false;

            db.SaveChanges();
        }
    }
}
