FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Shared
COPY ["Backend/Shared/Broker/Broker", "Broker/Broker/"]
WORKDIR /src
COPY ["Backend/Microservices/GameService/src/Game.Application/Game.Application.csproj", "src/Game.Application/"]
COPY ["Backend/Microservices/GameService/src/Game.Domain/Game.Domain.csproj", "src/Game.Domain/"]
COPY ["Backend/Microservices/GameService/src/Game.Data/Game.Data.csproj", "src/Game.Data/"]
COPY ["Backend/Microservices/GameService/src/Game.WebAPI/Game.WebAPI.csproj", "src/Game.WebAPI/"]
RUN dotnet restore "./src/Game.WebAPI/Game.WebAPI.csproj"
COPY ["Backend/Microservices/GameService/src", "src/"]
WORKDIR "/src/src/Game.WebAPI"
RUN dotnet build "./Game.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Game.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.WebAPI.dll"]