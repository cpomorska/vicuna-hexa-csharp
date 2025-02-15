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
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public RestErrorResponse Error()
        {
            var context = HttpContext?.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var code = 500;

            if (exception is UserNotFoundException)
            {
                code = 404;
            }
            else if (exception is UserUnauthException)
            {
                code = 401;
            }
            else if (exception is UserCreationException)
            {
                code = 400;
            }

            Response.StatusCode = code;
            var ex = new UserException(HttpStatusCode.NotFound, ErrorCode.Unknown, exception!.Message);
            return new RestErrorResponse(ex);
        }
    }
}