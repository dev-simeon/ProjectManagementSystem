using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Security.Claims;

namespace ProjectManagementSystem.Pages
{
    public class SignUpModel(PMSContext context) : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Mobile { get; set; }

        public PageResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var userExist = await CheckIfUserExistsAsync();
                if (userExist)
                {
                    ModelState.AddModelError("", "User with email already exists.");
                    return Page();
                }

                if (Password != ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return Page();
                }

                var user = CreateUser(FirstName, LastName, Email, Mobile, Password);

                context.Users.Add(user);
                await context.SaveChangesAsync();

                // Manually sign in the user after successful signup
                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("Projects");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Page();
        }

        private static User CreateUser(string firstName, string lastName, string email, string mobile, string password)
        {
            var user = new User(firstName, lastName, email, mobile, password);
            return user;
        }

        private async Task<bool> CheckIfUserExistsAsync()
        {
            var user = await context.Users.ToListAsync();
            return user.Any(u => u.Email == Email);
        }

    }
}
