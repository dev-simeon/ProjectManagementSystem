using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Services;
using System.Text;

public class ForgotPasswordModel(PMSContext context, EmailService emailService) : PageModel
{
    [BindProperty]
    public required string Email { get; set; }

    public PageResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await context.Users.Where(u => u.Email == Email).SingleOrDefaultAsync();

        if(user == null)
        {
            ModelState.AddModelError("", "Email not found");
            return Page();
        }

        //user.Password = GeneratePassword(12);
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();

            emailService.SendPasswordResetEmail(user);
            Console.WriteLine("Password reset email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user or sending email: {ex.Message}");
            return Page();
        }
        return RedirectToPage("/Index");
    }

    private static string GeneratePassword(int length)
    {
        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string digits = "0123456789";
        string specialChars = "!@#$%^&*()_+[]{}|;:,.<>?";

        string allChars = letters + digits + specialChars;

        StringBuilder passwordBuilder = new();
        Random random = new();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(allChars.Length);
            passwordBuilder.Append(allChars[randomIndex]);
        }

        return passwordBuilder.ToString();
    }
}
