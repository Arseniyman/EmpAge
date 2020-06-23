using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmpAge.Models;
using EmpAge.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmpAge.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDBContext _context;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            AppDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { Email = model.Email, UserName = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                string role = "";
                if (model.RoleName == "Соискатель")
                {
                    role = "applicant";
                }
                else if (model.RoleName == "Работодатель")
                {
                    role = "employer";
                }

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверка принадлежности URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаление аутентификационных куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task <IActionResult> PersonalPage()
        {
            var user = await _userManager.FindByIdAsync(
                User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if(await _userManager.IsInRoleAsync(user, "applicant"))
            {
                var summaries = _context.Summaries.Where(
                    f => f.ApplicantId == user.Id
                    ).ToList();
                return View(summaries);
            }

            if (await _userManager.IsInRoleAsync(user, "employer"))
            {
                var vacancies = _context.Vacancies.Where(
                    f => f.EmployerId == user.Id
                    ).ToList();
                return View(vacancies);
            }

            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                var moders = await _userManager.GetUsersInRoleAsync("moder");
                return View(moders);
            }
            
            return View();
        }
    }
}