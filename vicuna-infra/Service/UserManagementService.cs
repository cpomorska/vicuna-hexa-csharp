using vicuna_infra.Controllers;
using vicuna_infra.Repository;

namespace vicuna_infra.Service
{
    public class UserManagementService
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger _logger;

        public UserManagementService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RestUserController>();
            _userRepository = new UserRepository();
        }
    }
}
