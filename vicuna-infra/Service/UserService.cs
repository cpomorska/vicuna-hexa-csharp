using System.Collections.Immutable;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger _logger;

        public UserService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userRepository = new UserRepository();
        }

        public User? FindUser(UserDto userDto)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userRepository
                    .GetList(x => x.UserName == userDto.UserName && x.UserNumber == userDto.UserNumber && x.UserPass == userDto.UserPass && x.UserEnabled == userDto.UserEnabled).ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading all entries from user  | " + ex);
            }

            return userEntries?.FirstOrDefault();
        }

        public User? GetUserByEmail(string email)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
               userEntries = _userRepository
                    .GetList(x => x.UserEmail == email).ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading entries from user by email  | " + ex);
            }

            return userEntries?.FirstOrDefault();
        }

        public User? GetUserByUsernnameAndPassword(string userName, string password)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userRepository
                     .GetList(x => x.UserName == userName && x.UserPass == password).ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading entries from user by email  | " + ex);
            }

            return userEntries?.FirstOrDefault();
        }

        public User? GetUserByUsername(string username)
        {
            IEnumerable<User>? userEntries = null;
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userRepository
                    .GetList(x => x.UserName == username).ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading all entries from user  | " + ex);
            }

            return userEntries?.FirstOrDefault(); ;
        }
    }
}
