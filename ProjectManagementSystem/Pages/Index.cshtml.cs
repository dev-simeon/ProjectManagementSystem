using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

namespace ProjectManagementSystem.Pages
{
    public class IndexModel(PMSContext context) : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public PageResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await AuthenticateUserAsync();

                if (user != null)
                {
                    await SignInUserAsync(user);

                    return RedirectToPage("Projects");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ModelState.AddModelError("", "User details does not match, please check login details.");
            return Page();
        }


        private async Task<User?> AuthenticateUserAsync()
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == Email);
            if (user != null && user.VerifyPassword(Password))
            {
                return user;
            }
            return null;
        }

        private Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
        }
    }
}
