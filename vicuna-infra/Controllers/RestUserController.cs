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
    public class RestUserController : ControllerBase
    {
        private readonly ILogger<RestUserController> _logger;
        private readonly UserReadOnlyService _userService;

        public RestUserController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userService = new UserReadOnlyService(loggerFactory);
        }

        [HttpGet]
        [Route("user/{userdto}")]
        public User? FindUser(UserDto user)
        {
            var userFound = _userService.FindUser(user).Result;
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound,
                    $"User {user.UserName} not found")
                : userFound;
        }

        [HttpGet]
        [Route("byname/{username}")]
        public User? GetUserByName(string username)
        {
            var userFound = _userService.GetUserByUsername(username).Result;
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound,
                    $"User {username} not found")
                : userFound;
        }

        [HttpGet]
        [Route("bynamepw/{name}/{pass}")]
        public User? GetUserByUsernmaAndPassword(string username, string password)
        {
            var userFound = _userService.GetUserByUsernnameAndPassword(username, password).Result;
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound,
                    $"User {username} not found")
                : userFound;
        }

        [HttpGet]
        [Route("byemail/{email}")]
        public User? GetUserByEmail(string useremail)
        {
            var userFound = _userService.GetUserByEmail(useremail).Result;
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound,
                    $"User {useremail} not found")
                : userFound;
        }
    }
}