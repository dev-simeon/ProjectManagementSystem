//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using ProjectManagementSystem.Data;

//namespace ProjectManagementSystem.Pages
//{
//    public class Index1Model(PMSContext context) : PageModel
//    {
//        [BindProperty(SupportsGet = try)]
//        public int TaskId { get; set; }

//        public async Task<ActionResult> OnGet()
//        {
//            try
//            {
//                var task = await context.Tasks
//                    .Include(t => t.Feature)
//                    .FirstOrDefaultAsync(t => t.Id == TaskId);

//                if (task == null)
//                {
//                    return NotFound();
//                }

//                var serializedTask = JsonConvert.SerializeObject(task);
//                var jsonResult = new JsonResult(serializedTask);

//                return jsonResult;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error retrieving task details: {ex.Message}");
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }
//    }
//}
