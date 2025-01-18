using System.Collections.Immutable;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserReadOnlyService : IUserService
    {
        private readonly ILogger _logger;
        private readonly UserUserRepository _userUserRepository;

        public UserReadOnlyService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userUserRepository = new UserUserRepository();
        }

        public Task<User?> FindUser(UserDto userDto)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userUserRepository
                    .GetList(x =>
                        x.UserName == userDto.UserName && x.UserNumber == userDto.UserNumber &&
                        x.UserPass == userDto.UserPass && x.UserEnabled == userDto.UserEnabled).Result
                    .ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading all entries from user  | " + ex);
            }

            return Task.FromResult(userEntries?.FirstOrDefault());
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userUserRepository
                    .GetList(x => x.UserEmail == email).Result.ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading entries from user by email  | " + ex);
            }

            return await Task.FromResult(userEntries?.FirstOrDefault());
        }

        public async Task<User?> GetUserByUsernnameAndPassword(string userName, string password)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userUserRepository
                    .GetList(x => x.UserName == userName && x.UserPass == password).Result.ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading entries from user by email  | " + ex);
            }

            return await Task.FromResult(userEntries?.FirstOrDefault());
        }

        public Task<User?> GetUserByUsername(string username)
        {
            IEnumerable<User>? userEntries = null;
            try
            {
                _logger.LogInformation("Reading entries by Username");
                userEntries = _userUserRepository
                    .GetList(x => x.UserName == username).Result.ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading all entries from user  | " + ex);
            }

            return Task.FromResult(userEntries?.FirstOrDefault());
        }
    }
}