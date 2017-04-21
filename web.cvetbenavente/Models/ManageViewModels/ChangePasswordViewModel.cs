using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.cvetbenavente.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A password atual é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Atual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "A nova password é obrigatória.")]
        [StringLength(100, ErrorMessage = "A nova password tem de ter um mínimo de {2} e um máximo de {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("NewPassword", ErrorMessage = "A nova password não coincide com a sua confirmação.")]
        public string ConfirmPassword { get; set; }
    }
}
