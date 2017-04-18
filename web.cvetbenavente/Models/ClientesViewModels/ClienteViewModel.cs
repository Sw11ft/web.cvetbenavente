using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.ClientesViewModels
{
    public class ClienteViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O contacto telefónico é obrigatório.")]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "A morada é obrigatória.")]
        public string Morada { get; set; }
        [Required(ErrorMessage = "O código postal é obrigatório.")]
        public string CodPostal { get; set; }
        [Required(ErrorMessage = "A localdiade é obrigatória.")]
        public string Localidade { get; set; }

        public string Observacoes { get; set; }

        public bool Active { get; set; }
    }
}
