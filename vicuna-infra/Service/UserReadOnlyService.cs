using System.Collections.Immutable;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserReadOnlyService(ILoggerFactory loggerFactory) : IUserService
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<RestUserController>();
        private readonly UserUserRepository _userUserRepository = new();

        public Task<User?> FindUser(UserDto userDto)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries from Users");
                userEntries = _userUserRepository
                    .GetList(x =>
                        x.UserName == userDto.UserName && x.UserNumber == userDto.UserNumber &&
                        x.UserPass == userDto.UserPass && x.UserEnabled == userDto.UserEnabled).Result
                    .ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading all entries from Users");
            }

            return Task.FromResult(userEntries?.FirstOrDefault());
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by email");
                userEntries = _userUserRepository
                    .GetList(x => x.UserEmail == email).Result.ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading entries from Users by email");
            }

            return await Task.FromResult(userEntries?.FirstOrDefault());
        }

        public async Task<User?> GetUserByUsernnameAndPassword(string userName, string password)
        {
            IEnumerable<User> userEntries = new List<User>();
            try
            {
                _logger.LogInformation("Reading entries by Username and Password");
                userEntries = _userUserRepository
                    .GetList(x => x.UserName == userName && x.UserPass == password).Result.ToImmutableList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading entries from Users by username password");
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
                _logger.LogError(ex, "Error reading all entries from Users");
            }

            return Task.FromResult(userEntries?.FirstOrDefault());
        }
    }
}