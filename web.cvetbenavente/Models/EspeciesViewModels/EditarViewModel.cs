using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace web.cvetbenavente.Models.EspeciesViewModels
{
    public class EditarViewModel
    {
        public Especie Especie { get; set; }
        public IFormFile Imagem { get; set; }
    }
}
