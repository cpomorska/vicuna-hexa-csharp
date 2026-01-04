using Docker.DotNet;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;

namespace vicuna_infra_test.Controller;

public class RestControllerFixture : IAsyncLifetime
{
    private const string UnixSocketAddr = "unix:/var/run/docker.sock";

    public static PostgreSqlContainer? PostgresContainerTest { get; private set; }
    public static KeycloakContainer? KeycloakContainerTest { get; private set; }

    public async Task InitializeAsync()
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
        if (await IsContainerRunningAsync("tc-vicuna-pg", dockerEndpoint))
        {
            await PostgresContainerTest.StartAsync().ConfigureAwait(true);
        }

        //await PostgresContainerTest.StartAsync().ConfigureAwait(true);

        // Warten, bis der Container vollständig gestartet und bereit ist
    }

    public async Task DisposeAsync()
    {
        if (PostgresContainerTest != null && PostgresContainerTest.State == TestcontainersStates.Running)
        {
            await PostgresContainerTest.StopAsync().ConfigureAwait(false);
            await PostgresContainerTest.DisposeAsync().AsTask().ConfigureAwait(true);
        }
        if (KeycloakContainerTest != null)
        {
            await KeycloakContainerTest.StopAsync().ConfigureAwait(false);
            await KeycloakContainerTest.DisposeAsync().AsTask();
        }
    }
    
    private async Task<bool> IsContainerRunningAsync(string containerName, string dockerEndpoint)
    {
        var client = new DockerClientConfiguration(new Uri(dockerEndpoint)).CreateClient();

        // Überprüfen Sie, ob der Container bereits läuft
        var containers = await client.Containers.ListContainersAsync(new ContainersListParameters()
        {
            All = true
        });

        return containers.Any(c => c.Names.Contains("/" + containerName) && c.Status == "Up");
    }

}