using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace web.cvetbenavente.Models
{
    public class Especie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Remote("EspecieNameValid", "Especies", ErrorMessage = "Já existe uma espécie com este nome")]
        public string Nome { get; set; }

        public bool Active { get; set; }

        public string Imagem { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataEdicao { get; set; }
    }
}
