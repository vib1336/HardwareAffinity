namespace HardwareAffinity.Web.Extensions
{
    using System.Linq;
    using System.Security.Claims;

    public static class IdentityExtensions
    {
        /// <summary>
        /// Gets the Id of the current user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetId(this ClaimsPrincipal user)
            => user
            .Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
