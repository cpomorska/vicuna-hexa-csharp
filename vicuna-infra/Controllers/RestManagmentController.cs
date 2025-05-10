using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Response;
using vicuna_infra.Service;

namespace vicuna_infra.Controllers
{
    [ApiController]
    [Route("manage")]
    [EnableCors("DevelopmentPolicy")]
    [AllowAnonymous]
    public class RestUserManagementController : ControllerBase
    {
        private readonly ILogger<RestUserController> _logger;
        private readonly UserManagementService _userService;

        public RestUserManagementController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userService = new UserManagementService(loggerFactory);
        }

        [HttpPost]
        [Route("create")]
        public Guid AddUser(User user)
        {
            var userFoundGuid = _userService.AddUser(user).Result;
            _logger.LogInformation($"Adding user $user.UserNumber");
            return (Guid)(userFoundGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotCreated,
                    $"User {user.UserName} not created")
                : userFoundGuid);
        }

        [HttpPost]
        [Route("update")]
        public Guid UpdateUser(User user)
        {
            var userUpdateGuid = _userService.UpdateUser(user).Result;
            return (Guid)(userUpdateGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotUpdated,
                    $"User {user.UserName} not updated")
                : userUpdateGuid);
        }

        [HttpPost]
        [Route("remove")]
        public Guid RemoveUser(User user)
        {
            var userRemoveGuid = _userService.AddUser(user).Result;
            return (Guid)(userRemoveGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotRemoved,
                    $"User {user.UserName} not removed")
                : userRemoveGuid);
        }
    }
}