using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.ClientesViewModels
{
    public class CriarViewModel
    {
        public ClienteViewModel Cliente { get; set; }

        public List<Animal> Animais { get; set; }
    }
}
