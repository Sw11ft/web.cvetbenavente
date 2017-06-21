using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models
{
    public class Enums
    {
        public enum Genero
        {
            M,
            F
        }

        public enum TipoAtivo
        {
            Ambos,
            Ativo,
            Inativo
        }

        public enum OrderClientes
        {
            NoOrder,
            Nome,
            Contacto,
            Morada,
            Active
        }

        public enum OrderDirection
        {
            Asc,
            Desc
        }

        public enum Modelos
        {
            [Display(Name = "Lembrete de marcação")]
            [Description("Lembrete de marcação")]
            Lembrete,
            [Display(Name = "Marcação efetuada")]
            [Description("Marcação efetuada")]
            MarcacaoEfetuada
        }
    }
}
