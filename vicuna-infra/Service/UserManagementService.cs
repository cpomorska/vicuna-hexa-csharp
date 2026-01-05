using System.Collections.Immutable;
using vicuna_ddd.Domain.Users.Events;
using vicuna_ddd.Infrastructure.Events;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserManagementService(ILoggerFactory loggerFactory, IDomainEventDispatcher dispatcher) : IUserManagementService
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<RestUserController>();
        private readonly UserUserRepository _userUserRepository = new();
        private readonly IDomainEventDispatcher _dispatcher = dispatcher;

        public async Task<Guid?> AddUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Create user {UserNumber}", user.UserNumber);
                await _userUserRepository.Add(user);
                guid = user.UserNumber;

                // Domain-Event nach erfolgreichem Persistieren
                var evt = new UserCreatedEvent(user.UserNumber, user.UserName, DateTime.Now); // Passe Konstruktor an
                await _dispatcher.DispatchAsync(evt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error creating user {UserNumber}", user.UserNumber);
            }

            return guid;
        }

        // analog für UpdateUser / RemoveUser: nach Erfolg ein Event dispatchen
        public Task<Guid?> UpdateUser(User user)
        {
            Guid? guid = null;
            try
            {
                _logger.LogInformation("Update user {UserNumber}", user.UserNumber);
                _ = _userUserRepository.Update(user);
                guid = user.UserNumber;

                // Domain-Event nach erfolgreichem Persistieren
                var evt = new UserUpdatedEvent(user.UserNumber, user.UserName, DateTime.Now); // Passe Konstruktor an
                _dispatcher.DispatchAsync(evt);
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
                _ = _userUserRepository.Remove(user);
                guid = user.UserNumber;

                // Domain-Event nach erfolgreichem Persistieren
                var evt = new UserRemovedEvent(user.UserNumber, user.UserName, DateTime.Now); // Passe Konstruktor an
                _dispatcher.DispatchAsync(evt);
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

                // Domain-Event nach erfolgreichem Persistieren
                var evt = new UserRemovedEvent(user.UserNumber, user.UserName, DateTime.Now); // Passe Konstruktor an
                _dispatcher.DispatchAsync(evt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error removing user {UserId}", userId);
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