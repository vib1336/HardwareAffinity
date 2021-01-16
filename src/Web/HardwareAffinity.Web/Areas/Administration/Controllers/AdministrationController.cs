namespace HardwareAffinity.Web.Areas.Administration.Controllers
{
    using HardwareAffinity.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static HardwareAffinity.Common.GlobalConstants;

    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
