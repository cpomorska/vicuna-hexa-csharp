using DotNet.Testcontainers.Builders;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;

namespace vicuna_infra_test.Controller
{
    public class RestControllerFixture : IAsyncLifetime
    {
        private const string UnixSocketAddr = "unix:/var/run/docker.sock";

        public static PostgreSqlContainer? PostgresContainerTest { get; private set; }
        public static KeycloakContainer? KeycloakContainerTest { get; private set; }

        public RestControllerFixture()
        {
            _ = SetupClass();
        }

        public async Task SetupClass()
        {
            var dockerEndpoint = Environment.GetEnvironmentVariable("DOCKER_HOST") ?? UnixSocketAddr;

            PostgresContainerTest = new PostgreSqlBuilder()
                //.WithDockerEndpoint(dockerEndpoint)
                .WithEnvironment("POSTGRES_DB", "vicuna_pg")
                .WithEnvironment("POSTGRES_USER", "vicuna_user")
                .WithEnvironment("POSTGRES_PASSWORD", "vicuna_pw")
                .WithImage("postgres:15")
                .WithName("tc-vicuna-pg")
                .WithHostname("tc-vicuna-pg")
                .WithPortBinding(15432, 5432)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                .WithExtraHost("host.docker.internal", "host-gateway")
                .WithCleanUp(true)
                .Build();



            // KeycloakContainerTest = new KeycloakBuilder()
            //     //.WithDockerEndpoint(dockerEndpoint)
            //     .WithEnvironment("KEYCLOAK_ADMIN","admin")
            //     .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD","admin")
            //     .WithEnvironment("ADVERTISED_HOST","keycloak.host.internal")
            //     .WithEnvironment("ADVERTISED_PORT","28443")
            //     .WithPortBinding(28443, 443)
            //     .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            //     .WithExtraHost("keycloak.host.internal","host-gateway")
            //     .WithCleanUp(true)
            //     .Build();

            await PostgresContainerTest!.StartAsync().ConfigureAwait(false);
            // await KeycloakContainerTest!.StartAsync().ConfigureAwait(false);
        }


        public async Task InitializeAsync()
        {

        }

        public async Task DisposeAsync()
        {
            // await PostgresContainerTest!.StopAsync().ConfigureAwait(false);
            // await PostgresContainerTest.DisposeAsync().AsTask();
            //
            // await KeycloakContainerTest!.StopAsync().ConfigureAwait(false);
            // await KeycloakContainerTest.DisposeAsync().AsTask();
        }
    }
}
