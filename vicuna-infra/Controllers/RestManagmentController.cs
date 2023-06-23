using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Response;
using vicuna_infra.Service;

namespace vicuna_infra.Controllers
{
    [ApiController]
    [Route("manage")]
    [EnableCors("DevelopmentPolicy")]
    public class RestUserManagemnetController : ControllerBase
    {

        private readonly ILogger<RestUserController> _logger;
        private readonly UserManagementService _userService;

        public RestUserManagemnetController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userService = new UserManagementService(loggerFactory);
        }

        [HttpPost]
        [Route("create")]
        public Guid AddUser(User user)
        {
            var userFoundGuid = _userService.AddUser(user);
            return (Guid)(userFoundGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotCreated, $"User {user.UserName} not created")
                : userFoundGuid);
        }

        [HttpPost]
        [Route("update")]
        public Guid UpdateUser(User user)
        {
            var userUpdateGuid = _userService.UpdateUser(user);
            return (Guid)(userUpdateGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotUpdated, $"User {user.UserName} not updated")
                : userUpdateGuid);
        }

        [HttpPost]
        [Route("remove")]
        public Guid RemoveUser(User user)
        {
            var userRemoveGuid = _userService.AddUser(user);
            return (Guid)(userRemoveGuid == null
                ? throw new UserCreationException(HttpStatusCode.NotFound, ErrorCode.UserNotRemoved, $"User {user.UserName} not removed")
                : userRemoveGuid);
        }

    }
}