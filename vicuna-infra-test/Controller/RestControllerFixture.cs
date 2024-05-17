using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace vicuna_infra_test.Controller
{
    public class RestControllerFixture : IAsyncLifetime
    {
        private const string UnixSocketAddr = "unix:/var/run/docker.sock";

        public static PostgreSqlContainer PostgresContainer { get; private set; }

        public RestControllerFixture() 
        {
            _ = SetupClass();
        }

        public async Task SetupClass()
        {
            var dockerEndpoint = Environment.GetEnvironmentVariable("DOCKER_HOST") ?? UnixSocketAddr;

            PostgresContainer = new PostgreSqlBuilder()
                //.WithDockerEndpoint(dockerEndpoint)
                .WithEnvironment("POSTGRES_DB", "vicuna_pg")
                .WithEnvironment("POSTGRES_USER", "vicuna_user")
                .WithEnvironment("POSTGRES_PASSWORD", "vicuna_pw")
                .WithImage("postgres:15")
                .WithName("tc-vicuna-pg")
                .WithHostname("tc-vicuna-pg")
                .WithPortBinding(15432, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
                .WithExtraHost("host.docker.internal","host-gateway")
                .WithCleanUp(true)
                .Build();

            await PostgresContainer.StartAsync().ConfigureAwait(false);
        }


        public async Task InitializeAsync()
        {
            
        }

        public async Task DisposeAsync()
        {
             await PostgresContainer.StopAsync().ConfigureAwait(false);
             await PostgresContainer.DisposeAsync().AsTask();
        }
    }
}
