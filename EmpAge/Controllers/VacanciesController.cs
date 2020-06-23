using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpAge.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EmpAge.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<VacanciesController> _logger;

        public VacanciesController(AppDBContext context,
            UserManager<IdentityUser> userManager,
            ILogger<VacanciesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var vacancies = await _context.Vacancies.ToListAsync();

            if (String.IsNullOrEmpty(searchString))
            {
                return View(vacancies);
            }
            else
            {
                var compar = StringComparison.CurrentCultureIgnoreCase;

                return View(vacancies.Where(v =>
                    v.Description != null && v.Description.Contains(searchString, compar) ||
                    v.Name != null && v.Name.Contains(searchString, compar) ||
                    v.Location != null && v.Location.Contains(searchString, compar) ||
                    v.Salary != null && v.Salary.Contains(searchString, compar) ||
                    v.JobSector.ToString().Contains(searchString, compar) ||
                    v.EmploymentType.ToString().Contains(searchString, compar)
                ).ToList());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        [Authorize(Roles = "employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employer")]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Salary,Location,EmploymentType,JobSector,Description,EmployerId")] Vacancy vacancy)
        {
            vacancy.EmployerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction("PersonalPage", "Account");
            }
            return View(vacancy);
        }

        [Authorize(Roles = "employer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Salary,Location,EmploymentType,JobSector,Description,EmployerId")] Vacancy vacancy)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != vacancy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vacancy.EmployerId = currentUserId;
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogWarning("{0}, Warn, Problem with User {1} " +
                        "try to have edit vacancy - {2}",
                    DateTime.Now, User.Identity.Name, vacancy.Id);

                    if (!VacancyExists(vacancy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("PersonalPage", "Account");
            }
            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employer, moder")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser currentUser = await _userManager.FindByIdAsync(currentUserId);

            if(vacancy.EmployerId == currentUserId ||
                await _userManager.IsInRoleAsync(currentUser, "moder"))
            {
                _logger.LogInformation("{0}, Info, User {1} delete {2}`s vacancy - {3}",
                    DateTime.Now, User.Identity.Name, vacancy.EmployerId, vacancy.Name);
                
                _context.Vacancies.Remove(vacancy);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PersonalPage", "Account");
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancies.Any(e => e.Id == id);
        }
    }
}
