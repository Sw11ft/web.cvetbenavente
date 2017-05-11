using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.ClientesViewModels
{
    public class IndexViewModel
    {
        public List<Cliente> Clientes { get; set; }
        public int NrClientes { get; set; }
        public int Pagina { get; set; }
    }
}
