using DotNet.Testcontainers.Builders;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;

namespace vicuna_infra_test.Controller
{
    public class RestControllerFixture : IAsyncLifetime
    {
        public RestControllerFixture()
        {
            _ = SetupClass();
        }

        public static PostgreSqlContainer? PostgresContainerTest { get; private set; }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task SetupClass()
        {
            PostgresContainerTest = new PostgreSqlBuilder()
                //.WithDockerEndpoint(dockerEndpoint)
                .WithEnvironment("POSTGRES_DB", "vicuna_pg")
                .WithEnvironment("POSTGRES_USER", "vicuna_user")
                .WithEnvironment("POSTGRES_PASSWORD", "vicuna_pw")
                .WithImage("postgres:17")
                .WithName("tc-vicuna-pg")
                .WithHostname("tc-vicuna-pg")
                .WithPortBinding(15432, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .WithExtraHost("host.docker.internal", "host-gateway")
                .WithCleanUp(true)
                .Build();

            await PostgresContainerTest!.StartAsync().ConfigureAwait(false);
        }
    }
}