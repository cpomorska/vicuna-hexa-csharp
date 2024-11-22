#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["vicuna-infra/vicuna-infra.csproj", "vicuna-infra/"]
RUN dotnet restore "vicuna-infra/vicuna-infra.csproj"
COPY .. .
RUN dotnet build "vicuna-infra/vicuna-infra.csproj" -c Release --no-restore
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
RUN dotnet restore "vicuna-infra/vicuna-infra.csproj"
COPY . .
WORKDIR "/src/vicuna-infra"
RUN dotnet build "vicuna-infra.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vicuna-infra.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY /app/tls//tls.keycloak.crt /usr/local/share/ca-certificates/tls.keycloak.crt
WORKDIR /app
COPY --from=publish /app/publish .
RUN chmod 644 /usr/local/share/ca-certificates/tls.keycloak.crt && update-ca-certificates
ENTRYPOINT ["dotnet", "vicuna-infra.dll"]