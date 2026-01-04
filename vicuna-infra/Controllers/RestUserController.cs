using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Response;
using vicuna_infra.Service;

namespace vicuna_infra.Controllers
{
    [ApiController]
    [Route("read")]
    [EnableCors("DevelopmentPolicy")]
    public class RestUserController(ILoggerFactory loggerFactory) : ControllerBase
    {
        private readonly ILogger<RestUserController> _logger = loggerFactory.CreateLogger<RestUserController>();
        private readonly UserReadOnlyService _userService = new(loggerFactory);

        [HttpGet]
        [Route("user/{userdto}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> FindUser(UserDto user)
        {
            if (!TryValidateModel(user))
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Reading entries by Username");
            var userFound = _userService.FindUser(user).Result;
            
            return userFound != null 
                ? Ok(userFound) 
                : throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound, "user not found whíle searching by name");
        }

        [HttpGet]
        [Route("byname/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserByName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }
            _logger.LogInformation("Reading entries by Username");
            var userFound = _userService.GetUserByUsername(username).Result;
            
            return userFound != null 
                ? Ok(userFound) 
                : NotFound();
        }

        [HttpGet]
        [Route("bynamepw/{name}/{pass}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserByUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest();
            }
            _logger.LogInformation("Reading entries by Username and Password");
            var userFound = _userService.GetUserByUsernnameAndPassword(username, password).Result;
            
            return userFound != null 
                ? Ok(userFound) 
                : NotFound();
        }

        [HttpGet]
        [Route("byemail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserByEmail(string useremail)
        {
            if (string.IsNullOrWhiteSpace(useremail))
            {
                return BadRequest();
            }
            _logger.LogInformation("Reading entries by Email");
            var userFound = _userService.GetUserByEmail(useremail).Result;
            
            return userFound != null 
                ? Ok(userFound) 
                : NotFound();
        }
    }
}