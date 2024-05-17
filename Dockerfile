#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["vicuna-infra/vicuna-infra.csproj", "vicuna-infra/"]
RUN dotnet restore "vicuna-infra/vicuna-infra.csproj"
COPY . .
WORKDIR "/src/vicuna-infra"
RUN dotnet build "vicuna-infra.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vicuna-infra.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vicuna-infra.dll"]