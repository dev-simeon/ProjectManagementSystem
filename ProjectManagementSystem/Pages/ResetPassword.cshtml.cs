using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectManagementSystem.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
        }
    }
}
