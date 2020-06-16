using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле 'Email' обязательное")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательное")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле 'Подтверждение пароля' обязательное")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Тип пользователя")]
        [RegularExpression(@"(Соискатель|Работодатель)",
                ErrorMessage = "Необходимо выбрать Соискатель либо Работодатель")]
        public string RoleName { get; set; }
    }
}
