# vicuna-ddd

REST service with DDD (work in progres) example with C# .Net Core, should be latter a reference implementation (for me)

- Authentication with Keycloak and OICD works

- All Containers mann eh! do startup

- Keyclosk uses default sample realm

- change web origin and base url in keycloak to your local url (https://host:port)

- Open API works and authenticates

- Tests run (with in memory db)


How to start

- add keyloak.host.internal to your /etc/hosts
- add host.docker.internal to /etc/hosts
- enable .NET developer cert (have a look into Dockerfile and dockwr-compose.override.yml)
- build project (should work with VS Studio 2022 and Jetbrains Rider)
- start docker compose 
- debug your project with starting as vicuna-infa:https


