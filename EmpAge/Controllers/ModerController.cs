using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EmpAge.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmpAge.Models;

namespace EmpAge.Controllers
{
    [Authorize(Roles = "admin")]
    public class ModerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ModerController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ModerViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                
                
                if (result.Succeeded)
                {
                    var resultRole = await _userManager.AddToRoleAsync(user, "moder");
                    return RedirectToAction("PersonalPage", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
        
            return RedirectToAction("PersonalPage", "Account");
        }
    }
}