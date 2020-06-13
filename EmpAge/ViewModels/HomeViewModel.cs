using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpAge.Models;

namespace EmpAge.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Summary> Summaries { get; set; }
        public IEnumerable<Vacancy> Vacancies { get; set; }
    }
}
