using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        public ILogger<ErrorController> Logger { get; }

        public ErrorController(ILogger<ErrorController> logger)
        {
            Logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch(statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested can not be found";
                    Logger.LogWarning($"404 Error occured. Path {statusCodeResult.OriginalPath} code {statusCode}" +
                        $"QueryString = {statusCodeResult.OriginalQueryString}");
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QS = statusCodeResult.OriginalQueryString;
                    break;
 
            }
            return View("NotFound");  
        }

        [Route("CustomError")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");
            return View("Error");

        }
    }
}
