using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_ddd.Shared.Provider;
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
        public void init()
        {
            _user = new User();
            _user.UserName = "TestUser";
            _user.UserEmail = "testemail@test.de";
            _user.UserPass = "Tespass";
            _userRepository.Add(_user);
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
    }
}