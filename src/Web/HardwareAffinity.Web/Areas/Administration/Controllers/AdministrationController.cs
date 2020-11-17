namespace HardwareAffinity.Web.Areas.Administration.Controllers
{
    using HardwareAffinity.Common;
    using HardwareAffinity.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
