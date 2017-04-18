using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome de utilizador é obrigatório")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A password é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
