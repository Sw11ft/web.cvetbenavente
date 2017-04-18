using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static web.cvetbenavente.Models.Enums;

namespace web.cvetbenavente.Models
{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid IdCliente { get; set; }
        public string Nome { get; set; }
        public Genero Genero { get; set; }
        public Guid IdEspecie { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdEspecie")]
        public Especie Especie { get; set; }
    }
}
