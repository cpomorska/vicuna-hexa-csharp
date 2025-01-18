using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using vicuna_ddd.Shared.Provider;
using vicuna_infra.Repository;

namespace vicuna_infra_test.Controller
{
    public class RestManagementControllerTest : RestControllerFixture
    {
        private const string PostUriAddUser = "manage/create";
        private const string PostUriRemoveUser = "manage/remove";
        private const string PostUriUpdateUser = "manage/update";

        private HttpClient _httpClient;
        private UserUserRepository? _userRepository;

        public RestManagementControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        public void Setup()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();

            using (var client = new UserDbContext(false))
            {
                client.Database.EnsureCreated();
            }

            _userRepository = new UserUserRepository();
        }

        public void Teardown()
        {
            _userRepository = new UserUserRepository();
            var users = _userRepository.GetAll().Result;

            foreach (var user in users)
            {
                _ = _userRepository.Remove(user);
            }
        }

        [Fact]
        public async Task TestAddUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUser:inMann!");
            var response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            var rawResult = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(rawResult);
            Assert.IsType<Guid>(rawResult);
        }

        [Fact]
        public async Task TestRemoveUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMann!");
            var response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            var rawResult = await response.Content.ReadAsStringAsync();

            var endResponse = await _httpClient.PostAsJsonAsync(PostUriRemoveUser, testUser);
            var removeResult = JsonSerializer.Deserialize<Guid>(await endResponse.Content.ReadAsStringAsync());

            Assert.NotNull(rawResult);
            Assert.IsType<Guid>(removeResult);
        }

        [Fact]
        public async Task TestUpdateUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMann!");
            _ = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);

            var endResponse = await _httpClient.PostAsJsonAsync(PostUriUpdateUser, testUser);
            var updateResult = JsonSerializer.Deserialize<Guid>(await endResponse.Content.ReadAsStringAsync());

            Assert.NotNull(updateResult);
            Assert.IsType<Guid>(updateResult);
        }
    }
}