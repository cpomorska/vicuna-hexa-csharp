# Vicuna DDD


A reference implementation of a REST service using Domain-Driven Design (DDD) and Hexagonal Architecture with C# and .NET 10.

## Features

- **Domain-Driven Design (DDD):** Clear separation of domain logic, infrastructure, and application layers.
- **Hexagonal Architecture:** Decoupled core logic from external concerns.
- **Authentication:** Integrated with Keycloak using OpenID Connect (OIDC).
- **API Documentation:** Interactive OpenAPI (Swagger) support with authentication.
- **Containerized:** Full multi-container setup using Docker Compose.
- **Messaging:** Integration with Apache Kafka.
- **Persistence:** PostgreSQL for data storage (Infrastructure tests use In-Memory DB).
- **Testing:** Unit and integration tests included.

## Tech Stack


- **Backend:** C# 14, .NET 10
- **Database:** PostgreSQL 18
- **Identity Provider:** Keycloak 26.x
- **Message Broker:** Apache Kafka
- **Containerization:** Docker, Docker Compose
- **IDE Support:** Visual Studio 2022, JetBrains Rider

## Getting Started

### Prerequisites

- Docker and Docker Compose installed.
- .NET 10 SDK.
- Add the following entries to your `/etc/hosts` (or `C:\Windows\System32\drivers\etc\hosts` on Windows):
  ```text
  127.0.0.1 keycloak.host.internal
  127.0.0.1 host.docker.internal
  ```

### Installation & Setup

1.  **Enable .NET Developer Certificates:**
    Follow the instructions in the `Dockerfile` and `docker-compose.override.yml` to trust the development certificates.

2.  **Build the Project:**
    You can build the project using your preferred IDE (Visual Studio 2022 or JetBrains Rider) or via CLI:
    ```bash
    dotnet build
    ```

3.  **Start Infrastructure:**
    Launch all required services (PostgreSQL, Kafka, Keycloak, etc.) using Docker Compose:
    ```bash
    docker compose up -d
    ```

4.  **Keycloak Configuration:**
    - The setup uses a default sample realm provided in the `config` directory.
    - Ensure you update the **Web Origin** and **Base URL** in the Keycloak admin console to match your local environment URL (e.g., `https://localhost:7208`).

### Running the Application

- Debug the project by starting the `vicuna-infra:https` profile.
- Access the OpenAPI/Swagger UI to interact with the service.


## Kafka & Docker networking & Troubleshooting

If you run into Kafka connection errors (rdkafka/Confluent client) inside containers, the most common causes are Docker network configuration and Kafka advertised listeners. Use the guidance below to get a reliable local setup and debug connection issues.

### Common fixes

- Prefer the Docker service name inside containers: set `Kafka:Bootstrapper` to `kafka:9092` (not `localhost` or `host.docker.internal`) so clients bootstrap via the Docker network.
- Do not use `network_mode: bridge` for the `kafka` service together with a user-defined network � remove `network_mode` and let `kafka` join the project network (e.g. `vicuna-net`).
- Ensure Kafka's advertised listeners include the internal name used by containers. Example env variables in `kafka.base.env`:
  ```text
  KAFKA_LISTENERS=PLAINTEXT://:9092,PLAINTEXT_HOST://0.0.0.0:29092
  KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://kafka:9092,PLAINTEXT_HOST://host.docker.internal:29092
  KAFKA_INTER_BROKER_LISTENER_NAME=PLAINTEXT
  ```
  Adjust variables to match the Kafka image you are using.
- If you need host access to Kafka from the host machine, expose an external listener (e.g. `PLAINTEXT_HOST`) and map the port in `docker-compose` (`29092:9092`).

### Quick debug from inside the app container

1. Open a shell in the running app container:
   ```bash
   docker compose exec vicuna-infra sh
   ```
2. Check connectivity:
   ```bash
   ping -c 1 kafka
   nc -vz kafka 9092
   ```
3. Check Kafka logs if connection fails:
   ```bash
   docker compose logs kafka
   ```

### App configuration

- Set `Kafka:Bootstrapper` in `appsettings.Development.json` or environment variables for Docker Compose to `kafka:9092`.
- Example docker-compose env override for the app service:
  ```yaml
  environment:
    - Kafka__Bootstrapper=kafka:9092
  ```

### Notes

- `host.docker.internal` is available on Docker Desktop; on Linux you may need alternative host gateway settings.
- Use Docker healthchecks and `depends_on` to ensure services are healthy before your app starts.
- Keep the development README updated with the exact Kafka image env variables (`kafka.base.env`) used in this repository.

## Testing

Run tests via your IDE's test runner or using the command line:
```bash
dotnet test
```
Infrastructure tests are configured to use an In-Memory database by default.

---
*Note: This project is a work in progress.*


