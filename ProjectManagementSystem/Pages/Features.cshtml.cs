using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Code;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModel;
using System.Data;

namespace ProjectManagementSystem.Pages
{
    [Authorize]
    public class FeaturesModel : PageModel
    {
        private readonly PMSContext _context;
        private int _currUserId;

        public FeaturesModel(PMSContext context)
        {
            CollaboratorRoleList = GetCollaboratorRolesSelectList();
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;
        public bool IsProjectOwner { get; set; }
        public IEnumerable<CollaboratorViewModel> Collaborators { get; set; } = [];
        public List<SelectListItem> CollaboratorRoleList { get; set; } = [];

        public ICollection<Feature> Features { get; set; } = [];

        [BindProperty]
        public string FeatureName { get; set; } = string.Empty;
        [BindProperty]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        public CollaboratorRoles Role { get; set; }

        [BindProperty]
        public string SearchInput { get; set; } = string.Empty;


        public async Task<PageResult> OnGet()
        {
            try
            {
                _currUserId = User.GetCurrUserId();
                var project = await _context.Projects
                                               .Include(p => p.Features)
                                                .ThenInclude(f => f.Tasks)
                                               .Include(p => p.Collaborators)
                                               .Include(p => p.Users)
                                               .FirstOrDefaultAsync(p => p.Id == ProjectId)
                                               ?? throw new ArgumentNullException("Project Not found");
                ProjectName = project.Name;

                Features = project.Features;

                Collaborators = project.Users
                                 .Select(u => new CollaboratorViewModel(
                                     u.FullName,
                                     project.Collaborators.FirstOrDefault(c => c.UserId == u.Id).Role
                                 ));
                var userproject = project.Collaborators.FirstOrDefault(c => c.UserId == _currUserId);
                IsProjectOwner = userproject.IsProjectOwner();

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var project = await _context.Projects.Where(p => p.Id == ProjectId).SingleOrDefaultAsync();

                if(project != null)
                {
                    var feature = new Feature(project, FeatureName);

                    project.Features.Add(feature);
                    feature.AddDescription(Description);
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var project = await _context.Projects.FindAsync(ProjectId)
                                                ?? throw new ArgumentNullException($"Project with id: {ProjectId} not found");

                var collaborator = await _context.Users.FirstOrDefaultAsync(u => u.Email == SearchInput)
                                        ?? throw new ArgumentNullException($"Registered user with email: {SearchInput} not found");

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
