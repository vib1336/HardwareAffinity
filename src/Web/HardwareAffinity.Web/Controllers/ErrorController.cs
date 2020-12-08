namespace HardwareAffinity.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
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

            return this.View("NotFound");
        }
    }
}
