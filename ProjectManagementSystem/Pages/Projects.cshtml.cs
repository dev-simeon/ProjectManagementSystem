using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Code;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementSystem.Pages
{
    [Authorize]
    public class ProjectsModel(PMSContext context) : PageModel
    {
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Description { get; set; } = string.Empty;
        public List<Project> Projects { get; set; } = [];

        public async Task<ActionResult> OnGetAsync()
        {
            // Retrieve projects where the user is the owner or a collaborator
            Projects = await context.Projects
                .Include(p => p.Collaborators)
                .Include(p => p.Features)
                .Where(p => p.Collaborators.Any(up => up.UserId == User.GetCurrUserId()))
                .ToListAsync();

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var currUserId = User.GetCurrUserId();
                var currUser = await context.Users.FindAsync(currUserId);

                if (currUser != null)
                {
                    var project = new Models.Project(currUser, Name, Description, DateTime.UtcNow, DateTime.UtcNow.AddDays(1000));

                    context.Projects.Add(project);
                    await context.SaveChangesAsync();

                    TempData["ShowModal"] = "true";
                    TempData["SuccessMessage"] = "Project added successfully";

                    return RedirectToPage("Projects");
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to add project. " + ex.Message);
                return Page();
            }
        }
    }
}
