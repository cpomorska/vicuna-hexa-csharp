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
    public class RestUserManagementController(ILoggerFactory loggerFactory) : ControllerBase
    {
        private readonly ILogger<RestUserController> _logger = loggerFactory.CreateLogger<RestUserController>();
        private readonly UserManagementService _userService = new(loggerFactory);

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> AddUser(User user)
        {
            if (!TryValidateModel(user))
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation($"Adding user {user.UserNumber}");
            var userFoundGuid = _userService.AddUser(user).Result;

            return userFoundGuid != Guid.Empty 
                ? Created($"/create/{userFoundGuid}", userFoundGuid) 
                : BadRequest();
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Guid> UpdateUser(User user)
        {
            if (!TryValidateModel(user))
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Updating user {user.UserNumber}");
            var userUpdateGuid = _userService.UpdateUser(user).Result;
            
            return userUpdateGuid != Guid.Empty
                ? NoContent()
                : NotFound();
        }

        [HttpDelete]
        [Route("remove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Guid> RemoveUser(User user)
        {
            if (!TryValidateModel(user))
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Removing user {user.UserNumber}");
            var userRemoveGuid = _userService.RemoveUser(user).Result;
            
            return userRemoveGuid != Guid.Empty
                ? NoContent()
                : NotFound();
        }
        
        [HttpDelete]
        [Route("remove/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Guid> RemoveUser(Guid userId)
        {
            if (Guid.Empty == userId)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Removing user { userId}");
            var userRemoveGuid = _userService.RemoveUser(userId).Result;
            
            return userRemoveGuid != Guid.Empty
                ? NoContent()
                : NotFound();
        }
    }
}