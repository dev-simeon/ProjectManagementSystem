using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using ProjectManagementSystem.Code;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Threading.Tasks;
using Task = ProjectManagementSystem.Models.Task;

namespace ProjectManagementSystem.Pages
{
    public class TasksModel : PageModel
    {
        private readonly PMSContext _context;

        public TasksModel(PMSContext context)
        {
            _context = context;
            TaskPriorityList = GetTaskPrioritySelectList();
        }


        [BindProperty(SupportsGet = true)]
        public int FeatureId { get; set; }

        [BindProperty]
        public string TaskTitle { get; set; }

        [BindProperty]
        public string? Note { get; set; }

        [BindProperty]
        public DateTime TaskDueDate { get; set; }

        [BindProperty]
        public Priority TaskPriority { get; set; }
        [BindProperty]
        public List<SelectListItem> TaskPriorityList { get; set; } = [];

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

        public async Task<IActionResult> OnGetTaskDetailsAsync(int taskId)
        {
            try
            {
                var task = await _context.Tasks
                            .FirstOrDefaultAsync(t => t.Id == taskId);

                if (task == null)
                {
                    return NotFound();
                }

                var serializerSettings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var jsonString = JsonConvert.SerializeObject(task, serializerSettings);

                return Content(jsonString, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving task details: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        public async Task<ActionResult> OnPostAsync(int taskId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var feature = await _context.Features
                                            .Include(f => f.Tasks)
                                            .FirstOrDefaultAsync(f => f.Id == FeatureId)
                                            ?? throw new ArgumentNullException($"Feature with id: {FeatureId} not found");

                if (taskId <= 0)
                {
                    // Adding a new task
                    var newTask = new Task(feature, TaskTitle, TaskDueDate, Note, TaskPriority);
                    feature.Tasks.Add(newTask);

                    TempData["ShowModal"] = "true";
                    TempData["SuccessMessage"] = "Task added successfully";
                }
                else
                {
                    // Updating an existing task
                    var existingTask = feature.Tasks.FirstOrDefault(t => t.Id == taskId)
                                                            ?? throw new ArgumentException($"Task with id: {taskId} not found");

                    existingTask.UpdateNote(Note);
                    existingTask.UpdateDueDate(TaskDueDate);
                    existingTask.UpdatePriority(TaskPriority);
                    //existingTask.UpdateStatus()

                    TempData["ShowModal"] = "true";
                    TempData["SuccessMessage"] = "Task updated successfully";
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            return RedirectToPage("Tasks");
        }


        public List<SelectListItem> GetTaskPrioritySelectList()
        {
            var priority = Enum.GetValues(typeof(Priority))
                            .Cast<Priority>()
                            .Select(r => new SelectListItem
                            {
                                Value = ((int)r).ToString(),
                                Text = r.ToString()
                            })
                            .ToList();

            return priority;
        }
    }
}
