namespace HardwareAffinity.Web.Controllers
{
    using HardwareAffinity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Route("Identity/[controller]/[action]")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
            => this.userManager = userManager;

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail([FromQuery(Name = "Input.Email")] string email)
        {
            var user = this.userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            try
            {
                if (user != null)
                {
                    return this.Json(EmailIsInUse);
                }
            }
            catch
            {
            }

            return this.Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUserName([FromQuery(Name = "Input.UserName")] string userName)
        {
            var user = this.userManager.FindByNameAsync(userName).GetAwaiter().GetResult();
            if (user != null)
            {
                return this.Json(UserNameIsInUse);
            }

            return this.Json(true);
        }
    }
}
