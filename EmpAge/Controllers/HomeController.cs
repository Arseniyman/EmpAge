using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpAge.Models;
using EmpAge.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmpAge.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
            
        }

        public async Task<IActionResult> Index()
        {
            var summaries = await _context.Summaries.ToListAsync();
            var vacancies = await _context.Vacancies.ToListAsync();
            var sum = summaries.TakeLast(3).Reverse();
            var vac = vacancies.TakeLast(3).Reverse();

            HomeViewModel model = new HomeViewModel { Summaries = sum, Vacancies = vac };
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
