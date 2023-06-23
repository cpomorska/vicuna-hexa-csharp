using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Provider;
using vicuna_ddd.Shared.Util;
using vicuna_infra.Repository;

namespace vicuna_infra_test
{
    [TestClass]
    public class RestManagementControllerTest
    {
        private const string PostUriAddUser = "manage/create";
        private const string PostUriRemoveUser = "manage/remove";
        private const string PostUriUpdateUser = "manage/update";
        private const string RequestUriUser = "/read/byname/TestUser";

        private HttpClient _httpClient;
        private UserRepository _userRepository;
        private User _user;

        public RestManagementControllerTest() 
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();

            using (var client = new UserDbContext(false))
            {
                client.Database.EnsureCreated();
            }
        }

        [TestInitialize]
        public void setup()
        {
            _userRepository = new UserRepository();
            _user = createTestUser("TestUser");
            _userRepository.Add(_user);
        }

        [TestCleanup]
        public void teardown()
        {
            _userRepository = new UserRepository();
            var users = _userRepository.GetAll();
            
            foreach (var user in users)
            {
                _userRepository.Remove(user);
            }
        }


        [TestMethod]
        public async Task TestAddUserAsync()
        { 
            var testUser = createTestUser("NewUserMann!");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            string? rawResult = await response.Content.ReadAsStringAsync();
            
            Assert.IsNotNull(rawResult);
            Assert.IsInstanceOfType(Guid.Parse(rawResult.Replace('"', ' ').Trim()), _user.UserNumber.GetType());
        }

        [TestMethod]
        public async Task TestRemoveUserAsync()
        {
            var testUser = createTestUser("NewUserMann!");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            string? rawResult = await response.Content.ReadAsStringAsync();

            HttpResponseMessage endResponse = await _httpClient.PostAsJsonAsync(PostUriRemoveUser, testUser);
            string? removeResult = await endResponse.Content.ReadAsStringAsync();

            Assert.IsNotNull(rawResult);
            Assert.IsInstanceOfType(Guid.Parse(removeResult.Replace('"', ' ').Trim()), _user.UserNumber.GetType());
        }

        [TestMethod]
        public async Task TestUpdateUserAsync()
        {
            var testUser = createTestUser("NewUserMann!");
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            string? rawResult = await response.Content.ReadAsStringAsync();

            HttpResponseMessage endResponse = await _httpClient.PostAsJsonAsync(PostUriUpdateUser, testUser);
            string? updateResult = await endResponse.Content.ReadAsStringAsync();

            Assert.IsNotNull(updateResult);
            Assert.IsInstanceOfType(Guid.Parse(updateResult.Replace('"', ' ').Trim()), _user.UserNumber.GetType());
        }

        private User createTestUser(string username)
        {
            var randomSaltMann = HashUtil.GetRandomSalt(13);
            var guid = Guid.NewGuid();

            var userHash = new UserHash
            {
                saltField = randomSaltMann,
                hashField = HashUtil.CalculateCustomerHash(username, randomSaltMann),
            };

            var userRole = new UserRole
            {
                RoleName = "TestRole",
                RoleDescription = "TestDescription",
                RoleType = UserRoleTypes.Admin
            };

            return new User 
            {
                UserName = username,
                UserEmail = "testemail@test.de",
                UserPass = "Testpass",
                UserNumber = guid,
                UserToken = "userToken",
                UserHash = userHash,
                UserRole = userRole,
                UserEnabled = true
            };
        }
    }
}