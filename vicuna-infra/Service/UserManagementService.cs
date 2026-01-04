using System.Collections.Immutable;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserManagementService(ILoggerFactory loggerFactory) : IUserManagementService
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<RestUserController>();
        private readonly UserUserRepository _userUserRepository = new();

        public Task<Guid?> AddUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Create user {UserNumber}", user.UserNumber);
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error creating user {UserNumber}", user.UserNumber);
            }

            return Task.FromResult(guid);
        }

        public Task<Guid?> UpdateUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Update user {UserNumber}", user.UserNumber);
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error updating user {UserNumber}", user.UserNumber);
            }

            return Task.FromResult(guid);
        }

        public Task<Guid?> RemoveUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Remove user {UserNumber}", user.UserNumber);
                _ = _userUserRepository.Add(user);
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error removing user {UserNumber}", user.UserNumber);
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
                User user = userEntries.FirstOrDefault(defaultValue: new User());
                
                if (user.UserNumber == Guid.Empty) return Task.FromResult(guid);
                
                _logger.LogInformation("Remove user {UserNumber}", user.UserNumber);
                _ = _userUserRepository.Remove(user);
                
                guid = user.UserNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error removing user {UserId}", userId);
            }

            return Task.FromResult(guid);
        }
    }
}