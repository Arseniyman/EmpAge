using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.Models
{
    public class Proposal
    {
        [Key]
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        [Required]
        [Display(Name = "Вакансия")]
        [RegularExpression(@"[1-9]", 
            ErrorMessage = "Необходимо выбрать запись")]
        public int RecordId { get; set; }
        [Required(ErrorMessage = "Поле 'Сообщение' обязательное")]
        [Display(Name = "Сообщение")]
        public string Message { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
