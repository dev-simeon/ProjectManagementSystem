using System.Security.Claims;

namespace ProjectManagementSystem.Code
{
    public static class Extensions
    {
        public static int GetCurrUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userStringId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(userStringId);
        }
    }
}
