using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Code;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModel;
using System.Data;

namespace ProjectManagementSystem.Pages
{
    public class FeaturesModel : PageModel
    {
        private readonly PMSContext _context;

        public FeaturesModel(PMSContext context)
        {
            CollaboratorRoleList = GetCollaboratorRolesSelectList();
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public IEnumerable<CollaboratorViewModel> Collaborators { get; set; } = [];
        public List<SelectListItem> CollaboratorRoleList { get; set; } = [];

        public int CurrUserId { get; set; }

        public ICollection<Feature> Features { get; set; } = [];

        [BindProperty]
        public string FeatureName { get; set; }
        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public CollaboratorRoles Role { get; set; }

        [BindProperty]
        public string SearchInput { get; set; }


        public async Task<PageResult> OnGet()
        {
            try
            {
                CurrUserId = User.GetCurrUserId();

                var project = await _context.Projects
                                               .Include(p => p.Features)
                                               .Include(p => p.Collaborators)
                                               .Include(p => p.Users)
                                               .Where(p => p.Id == ProjectId)
                                               .FirstOrDefaultAsync();
                ProjectName = project.Name;

                Features = project.Features;
                Collaborators = project.Users
                    .Select(c => 
                        new CollaboratorViewModel(c.FullName, project.Collaborators
                                .Where(f => f.UserId == c.Id)
                                    .Select(r => r.Role)
                                        .First())
                    );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            return Page();
        }

        public async Task<ActionResult> OnGetSearchAsync(string query)
        {
            // Perform a database query to retrieve search results based on the query
            var results = await _context.Users
                                        .Where(u => u.Email.Contains(query))
                                        // Select the relevant data for the search result
                                        .Select(u => new { u.Email })
                                        .ToListAsync();

            return new JsonResult(results);
        }

        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                var project = await _context.Projects.Where(p => p.Id == ProjectId).SingleOrDefaultAsync();

                if(project != null)
                {
                    var feature = new Feature(project, FeatureName);

                    project.Features.Add(feature);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            return RedirectToPage("Features");
        }

        public async Task<ActionResult> OnPostAddCollaboratorsAsync()
        {
            try
            {
                var project = await _context.Projects
                                            .Where(p => p.Id == ProjectId)
                                            .SingleOrDefaultAsync();

                var collaborator = await _context.Users
                                                .Where(p => p.Email == SearchInput)
                                                .SingleOrDefaultAsync();

                project.AddCollaborator(collaborator, Role);

                await _context.SaveChangesAsync();
                
                TempData["ShowModal"] = "true";
                TempData["SuccessMessage"] = "Collaborator successfully added to the project!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            return RedirectToPage("Features");
        }

        public bool IfUserIsProjectOwner(int userId, int projectId)
        {
            return _context.UserProjects
                .Where(up => up.UserId == userId && up.ProjectId == projectId && up.Role == CollaboratorRoles.OWNER)
                .Any();
        }

        public List<SelectListItem> GetCollaboratorRolesSelectList()
        {
            var roles = Enum.GetValues(typeof(CollaboratorRoles))
                            .Cast<CollaboratorRoles>()
                            .Select(r => new SelectListItem
                            {
                                Value = ((int)r).ToString(),
                                Text = r.ToString()
                            })
                            .ToList();

            return roles;
        }
    }
}
