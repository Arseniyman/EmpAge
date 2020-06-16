using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EmpAge.Models;
using Microsoft.Extensions.Logging;

namespace EmpAge.Controllers
{
    [Authorize(Roles = "moder")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDBContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager,
            AppDBContext context, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IndexApplicants(string searchString)
        {
            var applicants = await _userManager.GetUsersInRoleAsync("applicant");

            if (String.IsNullOrEmpty(searchString))
            {
                return View(applicants);
            }
            else
            {
                return View(applicants.Where(a => a.Email.Contains(searchString)));
            }  
        }

        [HttpGet]
        public async Task<IActionResult> IndexEmployers(string searchString)
        {
            var employers = await _userManager.GetUsersInRoleAsync("employer");

            if (String.IsNullOrEmpty(searchString))
            {
                return View(employers);
            }
            else
            {
                return View(employers.Where(a => a.Email.Contains(searchString)));
            }   
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            string userEmail = user.Email;
            bool isDelUserEmployer = await _userManager.IsInRoleAsync(user, "employer");

            var result =  await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("{0}, Info, Moderator {1} delete {2}", 
                    DateTime.Now, User.Identity.Name, userEmail);

                if (isDelUserEmployer)
                {
                    var delUserVacancies = _context.Vacancies.Where(v =>
                    v.EmployerId == id);

                    _context.Vacancies.RemoveRange(delUserVacancies);
                    _context.SaveChanges();

                    return RedirectToAction("IndexEmployers", "User");
                }
                else
                {
                    var delUserSummaries = _context.Summaries.Where(s =>
                    s.ApplicantId == id);

                    _context.Summaries.RemoveRange(delUserSummaries);
                    _context.SaveChanges();

                    return RedirectToAction("IndexApplicants", "User");
                }
                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}