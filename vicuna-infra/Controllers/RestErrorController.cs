using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Shared.Response;

namespace vicuna_infra.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("error")]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorsController(ILogger<ErrorsController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet]
        public IActionResult Error()
        {
            var context = HttpContext?.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            if (exception is null)
            {
                return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: "Unknown error");
            }

            _logger.LogError(exception, "Unhandled exception caught by global handler");

            var status = exception switch
            {
                UserNotFoundException => HttpStatusCode.NotFound,
                UserUnauthException => HttpStatusCode.Unauthorized,
                UserCreationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var message = _env.EnvironmentName == "Development" ? exception.Message : "An unexpected error occurred.";
            var userEx = new UserException(status, ErrorCode.Unknown, message);

            Response.StatusCode = (int)status;
            return new ObjectResult(new RestErrorResponse(userEx)) { StatusCode = (int)status };
        }
    }
}