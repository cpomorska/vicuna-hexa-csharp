using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Domain.Users.Exceptions;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Repository;
using Xunit;

namespace vicuna_infra_test.Controller
{
    public class RestUserControllerTest
    {
        private const string RequestUriUserAsDto = "read/user/{userDto}";
        private const string RequestUriUserPass = "/read/bynamepw/Testuser/Testpass";
        private const string RequestUriUser = "/read/byname/TestUser";
        private const string RequestUriUserNotExist = "/read/byname/Loinin";
        private const string RequestUriEmail = "/read/byemail/testemail@test.de";

        private readonly HttpClient _httpClient;

        private UserRepository _userRepository;
        private User _user;

        public RestUserControllerTest()
        {
            var webFactory = new WebApplicationFactory<Program>();
            _httpClient = webFactory.CreateDefaultClient();
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
        }

        public void Setup()
        {
            _userRepository = new UserRepository();
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userRepository.Add(_user);
        }

        public void Teardown()
        {
            _userRepository = new UserRepository();
            var users = _userRepository.GetAll().Result;

            foreach (var user in users)
            {
                _ = _userRepository.Remove(user);
            }
        }

        [Fact]
        public async Task TestFindUserInDtoAsync()
        {
            UserDto userDto = new UserDto();
            userDto.UserPass = _user.UserPass;
            userDto.UserName = _user.UserName;
            userDto.UserEmail = _user.UserEmail;

            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUserAsDto);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByNameAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUser);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByUsernmaAndPasswordAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriUserPass);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByEmailAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(RequestUriEmail);
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserNotFoundlAsync()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.GetAsync(RequestUriUserNotExist);
                JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Assert.IsType<UserNotFoundException>(ex.GetType());
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode!);
            }
        }
    }
}