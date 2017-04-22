using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.cvetbenavente.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }
        public string Contacto { get; set; }

        public string Morada { get; set; }
        public string CodPostal { get; set; }
        public string Localidade { get; set; }

        public string Observacoes { get; set; }

        public bool Active { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
        public DateTime? DataDesativacao { get; set; }
    }
}
