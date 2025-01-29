
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y wget && \
    wget https://github.com/jwilder/dockerize/releases/download/v0.6.1/dockerize-linux-amd64-v0.6.1.tar.gz && \
    tar -C /usr/local/bin -xzvf dockerize-linux-amd64-v0.6.1.tar.gz && \
    rm dockerize-linux-amd64-v0.6.1.tar.gz

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Shared
COPY ["Backend/Shared/Broker/Broker", "Broker/Broker/"]
WORKDIR /src
COPY ["Backend/Microservices/AuthService/src/Auth.API/Auth.API.csproj", "src/Auth.API/"]
COPY ["Backend/Microservices/AuthService/src/Auth.Application/Auth.BLL.csproj", "src/Auth.Application/"]
COPY ["Backend/Microservices/AuthService/src/Auth.Infrastructure/Auth.DAL.csproj", "src/Auth.Infrastructure/"]
RUN dotnet restore "./src/Auth.API/Auth.API.csproj"
COPY ["Backend/Microservices/AuthService/src", "src/"]
WORKDIR "/src/src/Auth.API"
RUN dotnet build "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dockerize", "-wait", "tcp://authdb:1433", "-timeout", "60s", "dotnet", "Auth.API.dll"]
