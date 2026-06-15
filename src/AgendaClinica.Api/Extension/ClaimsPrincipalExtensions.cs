using System.Security.Claims;

namespace AgendaClinica.Api.Extension
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? user.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : null;
        }

        public static string? GetUserEmail(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Email);

        public static string? GetUserRole(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Role);
    }
}
