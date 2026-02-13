using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using vicuna_ddd.Shared.Provider;
using vicuna_infra.Repository;
using vicuna_ddd.Infrastructure.Events;
using Assert = Xunit.Assert;


namespace vicuna_infra_test.Controller
{
    public class RestManagementControllerTest : RestControllerBase          
    {
        private const string PostUriAddUser = "manage/create";
        private const string PostUriRemoveUser = "manage/remove";
        private const string PostUriUpdateUser = "manage/update";

        private HttpClient _httpClient;
        private UserUserRepository? _userRepository;

        public RestManagementControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // replace domain event dispatcher with a no-op implementation to avoid Kafka calls
                        services.RemoveAll<IDomainEventDispatcher>();
                        services.AddSingleton<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                    });
                });

            _httpClient = webAppFactory.CreateDefaultClient();
        }

        public void Setup()
        {
            var webAppFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<IDomainEventDispatcher>();
                        services.AddSingleton<IDomainEventDispatcher, NoOpDomainEventDispatcher>();
                    });
                });

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
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMannRemove!");
            var response = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);
            var rawResult = await response.Content.ReadAsStringAsync();

            var endResponse = await _httpClient.DeleteAsync(PostUriRemoveUser + "/" + testUser?.UserNumber);

            Assert.NotNull(rawResult);
            Assert.Equal(HttpStatusCode.NoContent, endResponse.StatusCode);
        }

        [Fact]
        public async Task TestUpdateUserAsync()
        {
            var testUser = RestControllerTestHelpers.CreateTestUser("NewUserMannUpdate!");
            _ = await _httpClient.PostAsJsonAsync(PostUriAddUser, testUser);

            var endResponse = await _httpClient.PutAsJsonAsync(PostUriUpdateUser, testUser);

            Assert.NotNull(endResponse);
            Assert.Equal(HttpStatusCode.NoContent, endResponse.StatusCode);
        }

        // No-op domain event dispatcher used in tests to prevent Kafka/network access
        private class NoOpDomainEventDispatcher : IDomainEventDispatcher
        {
            public Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class => Task.CompletedTask;
        }
    }
}