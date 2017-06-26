using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.EventosViewModels
{
    public class IndexViewModel
    {
        public class Mes
        {
            public int Valor { get; set; }
            public string Nome { get; set; }
            public List<Evento> Eventos { get; set; } = new List<Evento>();
        }

        public class Ano
        {
            public int Valor { get; set; }
            public List<Mes> Meses { get; set; } = new List<Mes>();
        }

        public List<Ano> Anos { get; set; } = new List<Ano>();
    }
}
