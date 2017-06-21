using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models
{
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O ID do evento é obrigatório")]
        [Display(Name = "ID do Evento")]
        public string IdEvento { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [Display(Name = "Descrição")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "O modelo de mensagem é obrigatório")]
        [Display(Name = "Modelo de mensagem")]
        public Enums.Modelos Modelo { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório")]
        [Remote("ClienteExistsByStringId", "Clientes", ErrorMessage = "O cliente especificado não existe")]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "O animal é obrigatório")]
        [Remote("AnimalBelongsToCliente", "Animais", AdditionalFields = "IdCliente", ErrorMessage = "O animal selecionado não pertence ao cliente especificado.")]
        public Guid IdAnimal { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória")]
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }

        public string Observacoes { get; set; }
    }
}
