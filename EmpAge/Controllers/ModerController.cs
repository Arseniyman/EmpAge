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
using Microsoft.Extensions.Logging;

namespace EmpAge.Controllers
{
    [Authorize(Roles = "admin")]
    public class ModerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ModerController> _logger;
        public ModerController(UserManager<IdentityUser> userManager,
            ILogger<ModerController> logger)
        {
            _userManager = userManager;
            _logger = logger;
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
                    _logger.LogInformation("{0}, Info, Admin {1} create moderator {2}", 
                        DateTime.Now, User.Identity.Name, model.Email);

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
            string moderEmail = user.Email;
            
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("{0}, Info, Admin {1} delete moderator {2}",
                       DateTime.Now, User.Identity.Name, moderEmail);
            }

            return RedirectToAction("PersonalPage", "Account");
        }
    }
}