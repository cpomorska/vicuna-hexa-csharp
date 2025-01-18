using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using vicuna_ddd.Domain.Users.Dto;
using vicuna_ddd.Model.Users.Entity;
using vicuna_infra.Repository;

namespace vicuna_infra_test.Controller
{
    public class RestUserControllerTest : IClassFixture<RestControllerBase>
    {
        private const string RequestUriUserAsDto = "read/user/{userDto}";
        private const string RequestUriUserPass = "/read/bynamepw/Testuser/Testpass";
        private const string RequestUriUser = "/read/byname/TestUser1";
        private const string RequestUriUserNotExist = "/read/byname/Teschtuser";
        private const string RequestUriEmail = "/read/byemail/testemail@test.de";
        private readonly HttpClient _httpClient;
        private readonly UserUserRepository _userUserRepository;
        private User _user;

        public RestUserControllerTest(RestControllerBase restControllerBase)
        {
            var webFactory = new WebApplicationFactory<Program>();
            _httpClient = webFactory.CreateDefaultClient();
            _userUserRepository = new UserUserRepository();
        }

        [Fact]
        public async Task TestFindUserInDtoAsync()
        {
            // Given
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userUserRepository.Add(_user);
            var userDto = new UserDto
            {
                UserPass = _user.UserPass,
                UserName = _user.UserName,
                UserEmail = _user.UserEmail
            };

            // When
            var response = await _httpClient.GetAsync(RequestUriUserAsDto);

            // Then
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByNameAsync()
        {
            // Given
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userUserRepository.Add(_user);

            // When
            var response = await _httpClient.GetAsync(RequestUriUser);

            // Then
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByUsernameAndPasswordAsync()
        {
            // Given
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userUserRepository.Add(_user);

            // When
            var response = await _httpClient.GetAsync(RequestUriUserPass);

            // Then
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }

        [Fact]
        public async Task TestGetUserByEmailAsync()
        {
            // Given
            _user = RestControllerTestHelpers.CreateTestUser("TestUser1");
            _ = _userUserRepository.Add(_user);

            // When
            var response = await _httpClient.GetAsync(RequestUriEmail);

            // Then
            var userResult = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult);
        }
    }
}