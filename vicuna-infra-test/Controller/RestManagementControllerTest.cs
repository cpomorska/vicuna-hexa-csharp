using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using vicuna_ddd.Shared.Provider;
using vicuna_infra.Repository;

namespace vicuna_infra_test.Controller
{
    [TestClass]
    public class RestManagementControllerTest : RestControllerFixture
    {
        private const string PostUriAddUser = "manage/create";
        private const string PostUriRemoveUser = "manage/remove";
        private const string PostUriUpdateUser = "manage/update";

        private HttpClient _httpClient;
        private UserRepository? _userRepository;

        public RestManagementControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [TestInitialize]
        public void Setup()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();

            using (var client = new UserDbContext(false))
            {
                client.Database.EnsureCreated();
            }

            _userRepository = new UserRepository();
        }

        [TestCleanup]
        public void Teardown()
        {
            _userRepository = new UserRepository();
            var users = _userRepository.GetAll().Result;

            foreach (var user in users)
            {
                _ = _userRepository.Remove(user);
            }
        }

        [TestMethod]
        public async Task TestAddUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMann!");
            var response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            var rawResult = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());

            Assert.IsNotNull(rawResult);
            Assert.IsInstanceOfType(rawResult, typeof(Guid));
        }

        [TestMethod]
        public async Task TestRemoveUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMann!");
            var response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            var rawResult = await response.Content.ReadAsStringAsync();

            var endResponse = await _httpClient.PostAsJsonAsync(PostUriRemoveUser, testUser);
            var removeResult = JsonSerializer.Deserialize<Guid>(await endResponse.Content.ReadAsStringAsync());

            Assert.IsNotNull(rawResult);
            Assert.IsInstanceOfType(removeResult, typeof(Guid));
        }

        [TestMethod]
        public async Task TestUpdateUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMann!");
            _ = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);

            var endResponse = await _httpClient.PostAsJsonAsync(PostUriUpdateUser, testUser);
            var updateResult = JsonSerializer.Deserialize<Guid>(await endResponse.Content.ReadAsStringAsync());

            Assert.IsNotNull(updateResult);
            Assert.IsInstanceOfType(updateResult, typeof(Guid));
        }
    }
}