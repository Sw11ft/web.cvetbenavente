using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.ClientesViewModels
{
    public class DetalhesViewModel
    {
        public Cliente Cliente { get; set; }
        public List<Animal> Animais { get; set; }
        public List<Evento> Eventos { get; set; }
    }
}
