using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EmpAge.Controllers
{
    [Authorize(Roles = "moder")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
            bool isDelUserEmployer = await _userManager.IsInRoleAsync(user, "employer");
            await _userManager.DeleteAsync(user);
            
            if (isDelUserEmployer)
            {
                return RedirectToAction("IndexEmployers", "User");

            }
            else
            {
                return RedirectToAction("IndexApplicants", "User");
            }
        }
    }
}