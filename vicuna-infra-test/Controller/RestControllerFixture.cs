using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace vicuna_infra_test.Controller
{
    public abstract class RestControllerFixture
    {
        private const string UnixSocketAddr = "unix:/var/run/docker.sock";

        public static PostgreSqlContainer PostgresContainer { get; private set; }


        [ClassInitialize]
        public async Task SetupClass()
        {
            var dockerEndpoint = Environment.GetEnvironmentVariable("DOCKER_HOST") ?? UnixSocketAddr;

            PostgresContainer = new PostgreSqlBuilder()
                .WithDockerEndpoint(dockerEndpoint)
                .WithEnvironment("POSTGRES_DB", "vicuna_pg")
                .WithEnvironment("POSTGRES_USER", "vicuna_user")
                .WithEnvironment("POSTGRES_PASSWORD", "vicuna_pw")
                .WithImage("postgres:15")
                .WithName("tc-vicuna-pg")
                .WithPortBinding(15432, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .WithCleanUp(true)
                .Build();


            await PostgresContainer.StartAsync().ConfigureAwait(true);
        }

        [ClassCleanup]
        public async Task TeardownClass()
        {
            await PostgresContainer.StopAsync().ConfigureAwait(false);
            await PostgresContainer.DisposeAsync();
        }
    }
}
