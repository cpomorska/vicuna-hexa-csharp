using Testcontainers.PostgreSql;

namespace vicuna_infra_test
{
    public class RestControllerFixture
    {
        private const string UnixSocketAddr = "unix:/var/run/docker.sock";

        public static PostgreSqlContainer PostgresContainer { get; private set; }

        [ClassInitialize]
        public async Task SetupClass()
        {
            var dockerEndpoint = Environment.GetEnvironmentVariable("DOCKER_HOST") ?? UnixSocketAddr;

            PostgresContainer = new PostgreSqlBuilder()
                //.WithDockerEndpoint(dockerEndpoint)
                .WithDatabase("vicuna_pg")
                .WithImage("postgres:latest")
                .WithExposedPort(15432)
                .WithName("tc-vicuna-pg")
                .WithPortBinding(5432)
                .WithCleanUp(true)
                .Build();

           await PostgresContainer.StartAsync().ConfigureAwait(false);
        }

        [ClassCleanup]
        public async Task TeardownClass()
        {
            await PostgresContainer.StopAsync().ConfigureAwait(false);
            await PostgresContainer.DisposeAsync();
        }
    }
}
