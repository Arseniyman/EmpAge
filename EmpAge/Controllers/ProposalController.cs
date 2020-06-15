using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EmpAge.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmpAge.ViewModels;

namespace EmpAge.Controllers
{
    [Authorize]
    public class ProposalController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDBContext _context;

        public ProposalController(UserManager<IdentityUser> userManager,
            AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "employer, applicant")]
        public async Task<IActionResult> Create()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            SendProposalViewModel model;

            if (await _userManager.IsInRoleAsync(currentUser, "employer"))
            {
                var vacancies = _context.Vacancies.Where(v =>
                v.EmployerId == currentUserId);

                model = new SendProposalViewModel { Vacancies = vacancies };
            }
            else
            {
                var summaries = _context.Summaries.Where(s =>
                s.ApplicantId == currentUserId);

                model = new SendProposalViewModel { Summaries = summaries };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employer, applicant")]
        public async Task<IActionResult> Create([Bind("Id, SenderId, RecipientId, RecordId, Message")]
        Proposal proposal)
        {
            int i = 0;
            if (!int.TryParse(RouteData.Values["id"].ToString(), out i))
            {
                return NotFound();
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            proposal.SenderId = currentUserId;

            int recordId = int.Parse(RouteData.Values["id"].ToString());
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            
            SendProposalViewModel model;

            bool isEmployer = await _userManager.IsInRoleAsync(currentUser, "employer");

            if (isEmployer)
            {
                var summary = await _context.Summaries.FindAsync(recordId);
                
                if(summary == null)
                {
                    return NotFound();
                }

                proposal.RecipientId = summary.ApplicantId;
                var vacancies = _context.Vacancies.Where(v =>
                v.EmployerId == currentUserId);

                model = new SendProposalViewModel { Vacancies = vacancies };
            }
            else
            {
                var vacancy = await _context.Vacancies.FindAsync(recordId);

                if(vacancy == null)
                {
                    return NotFound();
                }

                proposal.RecipientId = vacancy.EmployerId;
                var summaries = _context.Summaries.Where(s =>
                s.ApplicantId == currentUserId);
                model = new SendProposalViewModel { Summaries = summaries };
            }

            proposal.CreateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(proposal);
                await _context.SaveChangesAsync();

                if (isEmployer)
                {
                    return RedirectToAction("Details", "Summaries", new { id = recordId });
                }

                return RedirectToAction("Details", "Vacancies", new { id = recordId });
            }

            model.Proposal = proposal;

            return View(model);
        }
        
        [Authorize(Roles = "employer, applicant")]
        public IActionResult Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var proposals = _context.Proposals.Where(p =>
                p.SenderId != null && p.SenderId == currentUserId || 
                p.RecipientId != null && p.RecipientId == currentUserId );

            return View(proposals.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employer, applicant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var proposal = await _context.Proposals.FindAsync(id);

            if(currentUserId != proposal.RecipientId && 
                currentUserId != proposal.SenderId)
            {
                return NotFound();
            }

            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Proposal");
        }
    }
}