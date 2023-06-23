using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
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
    public class RestUserControllerTest
    {
        private const string RequestUriUserAsDto = "read/user/{userDto}";
        private const string RequestUriUserPass = "/read/bynamepw/Testuser/Testpass";
        private const string RequestUriUser = "/read/byname/TestUser";
        private const string RequestUriUserNotExist = "/read/byname/Loinin";
        private const string RequestUriEmail = "/read/byemail/testemail@test.de";

        private HttpClient _httpClient;
        private UserRepository _userRepository;
        private User _user;

        public RestUserControllerTest() 
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();

            using (var client = new UserDbContext(false))
            {
                client.Database.EnsureCreated();
            }

            _userRepository = new UserRepository();
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
        public async Task TestFindUserInDtoAsync()
        {
            UserDto userDto = new UserDto();
            userDto.UserPass = _user.UserPass;
            userDto.UserName = _user.UserName;
            userDto.UserEmail = _user.UserEmail;

            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUserAsDto);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(userResult);
            Assert.IsInstanceOfType(userResult, _user.GetType());
        }

        [TestMethod]
        public async Task TestGetUserByNameAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUser);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(userResult);
            Assert.IsInstanceOfType(userResult, _user.GetType());
        }

        [TestMethod]
        public async Task TestGetUserByUsernmaAndPasswordAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUserPass);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(userResult);
            Assert.IsInstanceOfType(userResult, _user.GetType());
        }

        [TestMethod]
        public async Task TestGetUserByEmailAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriEmail);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(userResult);
            Assert.IsInstanceOfType(userResult, _user.GetType());
        }

        [TestMethod]
        public async Task TestGetUserNotFoundlAsync()
        {
            HttpResponseMessage? response = null;
            try
            {
                response = await _httpClient.GetAsync(RequestUriUserNotExist);
                var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            } catch (Exception ex) {
                Assert.IsInstanceOfType(ex.GetType(), typeof(UserNotFoundException));
                Assert.AreEqual(response?.StatusCode, 404);
            }
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