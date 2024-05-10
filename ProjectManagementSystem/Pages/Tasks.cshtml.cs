using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Code;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using Task = ProjectManagementSystem.Models.Task;

namespace ProjectManagementSystem.Pages
{
    public class TasksModel : PageModel
    {
        private readonly PMSContext _context;

        public TasksModel(PMSContext context)
        {
            _context = context;
        }


        [BindProperty(SupportsGet = true)]
        public int FeatureId { get; set; }

        [BindProperty]
        public string TaskTitle { get; set; }

        [BindProperty]
        public DateTime TaskDueDate { get; set; }

        [BindProperty]
        public Priority TaskPriority { get; set; }

        public bool IsProjectOwner { get; set; }

        public ICollection<Task> Tasks { get; set; } = [];

        public async Task<PageResult> OnGetAsync()
        {
            try
            {
                var feature = await _context.Features
                                            .Include(x => x.Tasks)
                                            .Include(x => x.Project)
                                            .ThenInclude(x => x.Collaborators)
                                            .FirstOrDefaultAsync(f => f.Id == FeatureId);


                Tasks = feature.Tasks;

                var userProject = feature.Project.Collaborators
                                         .FirstOrDefault(c => c.UserId == User.GetCurrUserId());
                IsProjectOwner = userProject.IsProjectOwner();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            
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
                var feature = await _context.Features.FindAsync(FeatureId) 
                                                ?? throw new ArgumentNullException($"Feature with id: {FeatureId} not found");

                var newTask = new Task(feature, TaskTitle, TaskDueDate, TaskPriority);

                feature.Tasks.Add(newTask);
                await _context.SaveChangesAsync();

                TempData["ShowModal"] = "true";
                TempData["SuccessMessage"] = "Task added successfully";

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            return RedirectToPage("Tasks");
        }
    }
}
