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
    [Route("read")]
    [EnableCors("DevelopmentPolicy")]
    public class RestUserController : ControllerBase
    {

        private readonly ILogger<RestUserController> _logger;
        private readonly UserService _userService;

        public RestUserController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userService = new UserService(loggerFactory);
        }

        [HttpGet]
        [Route("user/{userdto}")]
        public User? FindUser(UserDto user)
        {
            var userFound = _userService.FindUser(user);
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound, $"User {user.UserName} not found")
                : userFound;
        }

        [HttpGet]
        [Route("byname/{name}")]
        public User? GetUserByName(string name)
        {
            var userFound = _userService.GetUserByUsername(name);
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound, $"User {name} not found")
                : userFound;
        }

        [HttpGet]
        [Route("bynamepw/{name}/{pass}")]
        public User? GetUserByUsernmaAndPassword(string username, string password)
        {
            var userFound = _userService.GetUserByUsernnameAndPassword(username, password);
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound, $"User {username} not found")
                : userFound;
        }

        [HttpGet]
        [Route("byemail/{email}")]
        public User? GetUserByEmail(string email)
        {
            var userFound = _userService.GetUserByEmail(email);
            return userFound == null
                ? throw new UserNotFoundException(HttpStatusCode.NotFound, ErrorCode.UserNotFound, $"User {email} not found")
                : userFound;
        }
    }
}