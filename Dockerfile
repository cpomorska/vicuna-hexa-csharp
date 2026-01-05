#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
COPY ./tls/tls.keycloak.crt /app/tls/
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
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
RUN cp /app/tls/tls.keycloak.crt /usr/local/share/ca-certificates/tls.keycloak.crt
RUN chmod 644 /usr/local/share/ca-certificates/tls.keycloak.crt
RUN update-ca-certificates


# Erzeuge self-signed cert + pfx (Passwort hier als Beispiel 'changeit' — bei Bedarf ändern)
RUN mkdir -p /app/tls \
 && openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /app/tls/https.key -out /app/tls/https.crt -subj "/CN=localhost" \
 && openssl pkcs12 -export -out /app/tls/https.pfx -inkey /app/tls/https.key -in /app/tls/https.crt -passout pass:changeit \
 && chmod 644 /app/tls/https.pfx

# Kestrel so konfigurieren, dass das PFX verwendet wird
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/tls/https.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=changeit

RUN ls -la /app/tls

ENTRYPOINT ["dotnet", "vicuna-infra.dll"]