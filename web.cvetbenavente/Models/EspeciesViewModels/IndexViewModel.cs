using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.EspeciesViewModels
{
    public class IndexViewModel
    {
        public class Especie
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public int NrAnimais { get; set; }
        }

        public List<Especie> Especies { get; set; } = new List<Especie>();

        public int NrEspecies { get; set; }
        public int RegistosPorPagina { get; set; }
    }
}
