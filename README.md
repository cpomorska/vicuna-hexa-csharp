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

## Testing

Run tests via your IDE's test runner or using the command line:
```bash
dotnet test
```
Infrastructure tests are configured to use an In-Memory database by default.

---
*Note: This project is a work in progress.*


