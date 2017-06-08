using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static web.cvetbenavente.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace web.cvetbenavente.Models
{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Remote("ClienteExistsByStringId", "Clientes", ErrorMessage = "O cliente especificado não existe")]
        [Required(ErrorMessage = "O cliente é obrigatório")]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome", Description = "Nome do animal", ShortName = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O género é obrigatório")]
        [Display(Name = "Género", Description = "Género do animal")]
        public Genero Genero { get; set; }

        [Remote("EspecieExistsByStringId", "Especies", ErrorMessage = "A espécie especificada não existe")]
        [Required(ErrorMessage = "A espécie é obrigatória")]
        public Guid IdEspecie { get; set; }

        [Display(Name = "Observações", Description = "Indicações adicionais", ShortName = "Obs")]
        public string Observacoes { get; set; }

        [Display(Name = "Data de Criação", Description = "Data de quando o animal foi adicionado à plataforma")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data de Edição", Description = "Data da ultima alteração efetuada")]
        public DateTime? DataEdicao { get; set; }

        public bool Removido { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdEspecie")]
        public Especie Especie { get; set; }
    }
}
