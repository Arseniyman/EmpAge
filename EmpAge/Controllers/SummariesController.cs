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

namespace EmpAge.Controllers
{
    public class SummariesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SummariesController(AppDBContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
