using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.Models
{
    public class Summary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Заработная плата")]
        public string Salary { get; set; }
        [Display(Name = "Место проживания")]
        public string Location { get; set; }
        [Display(Name = "Образование")]
        public string Education { get; set; }
        [Display(Name = "Тип занятости")]
        public EmploymentType EmploymType { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public string ApplicantId { get; set; }
    }
}
