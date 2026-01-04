using System.Collections.Immutable;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ILogger _logger;
        private readonly UserUserRepository _userUserRepository;

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
                _logger.LogInformation($"Remove user {user.UserNumber}");
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing user {user.UserNumber} | " + ex);
            }

            return Task.FromResult(guid);
        }
        
        public Task<Guid?> RemoveUser(Guid userId)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Reading entries by Username");
                IEnumerable<User> userEntries = _userUserRepository
                    .GetList(x => x.UserNumber == userId).Result.ToImmutableList();
                User user = userEntries.FirstOrDefault();
                
                if (user == null) return Task.FromResult(guid);
                
                _logger.LogInformation($"Remove user {user.UserNumber}");
                _ = _userUserRepository.Remove(user);
                
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing user {userId} | " + ex);
            }

            return Task.FromResult(guid);
        }
    }
}