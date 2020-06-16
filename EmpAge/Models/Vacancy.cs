using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле 'Название' обязательное")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Заработная плата")]
        public string Salary { get; set; }
        [Display(Name = "Адрес")]
        public string Location { get; set; }
        [Display(Name = "Тип занятости")]
        public EmploymentType EmploymentType { get; set; }
        [Display(Name = "Область занятости")]
        public JobSector JobSector { get; set; }
        [Required(ErrorMessage = "Поле 'Описание' обязательное")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public string EmployerId { get; set; }
    }
}
