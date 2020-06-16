using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpAge.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EmpAge.Controllers
{
    public class SummariesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SummariesController> _logger;

        public SummariesController(AppDBContext context,
            UserManager<IdentityUser> userManager,
            ILogger<SummariesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var summaries = await _context.Summaries.ToListAsync();

            if (String.IsNullOrEmpty(searchString))
            {
                return View(summaries);
            }
            else
            {
                var compar = StringComparison.CurrentCultureIgnoreCase;

                return View(summaries.Where(s =>
                    s.Salary != null && s.Salary.Contains(searchString, compar) ||
                    s.Name != null && s.Name.Contains(searchString, compar) ||
                    s.Location != null && s.Location.Contains(searchString, compar) ||
                    s.Description != null && s.Description.Contains(searchString, compar) ||
                    s.EmploymType.ToString().Contains(searchString, compar)
                ));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var summary = await _context.Summaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (summary == null)
            {
                return NotFound();
            }

            return View(summary);
        }

        [Authorize(Roles = "applicant")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "applicant")]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Salary,Location,Education,EmploymType,Description,ApplicantId")] Summary summary)
        {
            summary.ApplicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(summary);
                await _context.SaveChangesAsync();
                return RedirectToAction("PersonalPage", "Account");
            }
            return View(summary);
        }

        [Authorize(Roles = "applicant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var summary = await _context.Summaries.FindAsync(id);
            if (summary == null)
            {
                return NotFound();
            }

            return View(summary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "applicant")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Salary,Location,Education,EmploymType,Description,ApplicantId")] Summary summary)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (id != summary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    summary.ApplicantId = currentUserId;
                    _context.Update(summary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogWarning("{0}, Warn, Problem with User {1} " +
                        "try to have edit summary - {2}",
                    DateTime.Now, User.Identity.Name, summary.Id);

                    if (!SummaryExists(summary.Id))
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
            return View(summary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "applicant, moder")]
        public async Task<IActionResult> Delete(int id)
        {
            var summary = await _context.Summaries.FindAsync(id);
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser currentUser = await _userManager.FindByIdAsync(currentUserId);

            if (summary.ApplicantId == currentUserId || 
                await _userManager.IsInRoleAsync(currentUser, "moder"))
            {
                _logger.LogInformation("{0}, Info, User {1} delete {2}`s summary - {3}",
                    DateTime.Now, User.Identity.Name, summary.ApplicantId, summary.Name);

                _context.Summaries.Remove(summary);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("PersonalPage", "Account");
        }

        private bool SummaryExists(int id)
        {
            return _context.Summaries.Any(e => e.Id == id);
        }
    }
}
