using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserUserRepository _userUserRepository;
        private readonly ILogger _logger;

        public UserManagementService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userUserRepository = new UserUserRepository();
        }

        public Task<Guid?> AddUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation($"Create user {user.UserNumber}");
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user {user.UserNumber} | " + ex);
            }
            return Task.FromResult(guid);
        }

        public Task<Guid?> UpdateUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation($"Update user {user.UserNumber}");
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {user.UserNumber} | " + ex);
            }

            return Task.FromResult(guid);
        }

        public Task<Guid?> RemoveUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation($"Update user {user.UserNumber}");
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing user {user.UserNumber} | " + ex);
            }

            return Task.FromResult(guid);
        }
    }
}
