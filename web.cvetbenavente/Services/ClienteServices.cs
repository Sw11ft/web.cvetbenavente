using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;
using static web.cvetbenavente.Models.Enums;

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
        public List<Cliente> GetClientes(TipoAtivo? TipoAtivo = TipoAtivo.Ambos)
        {
            var query = db.Clientes.AsQueryable();

            //Converte TipoAtivo? para TipoAtivo
            //Caso TipoAtivo? seja nulo, o valor atribuido
            //é o valor default (ou seja, no caso dos enums, o primeiro valor)
            var xTipoAtivo = TipoAtivo ?? default(TipoAtivo);

            if (xTipoAtivo == Enums.TipoAtivo.Ativo)
            {
                query = query.Where(x => x.Active == true);
            }
            else if (xTipoAtivo == Enums.TipoAtivo.Inativo)
            {
                query = query.Where(x => x.Active == false);
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
