using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Repository;

namespace vicuna_infra_test.Controller
{
    public class RestControllerBase : IDisposable
    {
        private readonly User? _user;
        private readonly UserUserRepository _userUserRepository;

        public RestControllerBase()
        {
            _userUserRepository = new UserUserRepository();
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userUserRepository.Add(_user);
        }

        public void Dispose()
        {
            var users = _userUserRepository.GetAll().Result;
            foreach (var user in users)
            {
                _ = _userUserRepository.Remove(user);
            }
        }
    }
}