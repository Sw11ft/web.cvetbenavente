using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.EspeciesViewModels
{
    public class DetalhesViewModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }
        public string NomeF { get; set; }
        public string Imagem { get; set; }
        public DateTime DataCriacao { get; set; }

        public int NrAnimais { get; set; }
    }
}
