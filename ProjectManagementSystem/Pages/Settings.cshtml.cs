using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Code;
using ProjectManagementSystem.Data;
using System.Security.Claims;

namespace ProjectManagementSystem.Pages
{
    [Authorize]
    public class SettingsModel(PMSContext context) : PageModel
    {
        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string OldNumber { get; private set; }

        [BindProperty]
        public string NewNumber { get; private set; }


        public PageResult OnGet()
        {
            return Page();
        }

        public async Task<ActionResult> OnPostAsync() 
        {
            try
            {
                var currAuthUser = await context.Users
                                                .Where(u => u.Id == User.GetCurrUserId())
                                                .SingleOrDefaultAsync();

                if (currAuthUser == null)
                {
                    // if user is null means user does not exists in the session 
                    //redirect to the login page
                    RedirectToPage("Index");
                }

                if (OldPassword != currAuthUser.Password)
                {
                    ModelState.AddModelError("", "Old password does not match cross check.");
                    return Page();
                }

                if (NewPassword == ConfirmPassword)
                {
                    currAuthUser.UpdatePassword(OldPassword, NewPassword);
                    context.Users.Update(currAuthUser);
                    await context.SaveChangesAsync();

                    // Set TempData to show the modal
                    TempData["ShowModal"] = "true";
                    TempData["SuccessMessage"] = "Password updated successfully!";
                    return RedirectToPage("Settings");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            
            ModelState.AddModelError("", "invalid password combination.");
            return Page();
        }

        public async Task<ActionResult> OnPostUpdateNumberAsync()
        {
            try
            {
                var currAuthUser = await context.Users
                                                .Where(u => u.Id == User.GetCurrUserId())
                                                .SingleOrDefaultAsync();

                if (currAuthUser != null)
                {
                    if (OldNumber != currAuthUser.Mobile)
                    {
                        ModelState.AddModelError("", "Incorrect number, please cross check.");
                        return Page();
                    }

                    //currAuthUser.Mobile = NewNumber;
                    context.Users.Update(currAuthUser);
                    await context.SaveChangesAsync();

                    // Set TempData to show the modal
                    TempData["ShowModal"] = "true";
                    TempData["SuccessMessage"] = "Password updated successfully!";
                    return RedirectToPage("Settings");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ModelState.AddModelError("", "invalid password combination.");
            return Page();
        }
    }
}
