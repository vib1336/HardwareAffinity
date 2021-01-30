namespace HardwareAffinity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    this.ViewBag.ErrorMessage = "Sorry, the resourse you requested could not be found";
                    break;
            }

            this.Response.StatusCode = 404;
            return this.View("NotFound");
        }
    }
}
