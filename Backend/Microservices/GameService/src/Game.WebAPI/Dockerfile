#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shared/Broker/Broker/Broker.csproj", "src/Broker/"]
COPY ["Microservices/GameService/src/Game.WebAPI/Game.WebAPI.csproj", "src/Game.WebAPI/"]
COPY ["Microservices/GameService/src/Game.Application/Game.Application.csproj", "src/Game.Application/"]
COPY ["Microservices/GameService/src/Game.Domain/Game.Domain.csproj", "src/Game.Domain/"]
COPY ["Microservices/GameService/src/Game.Data/Game.Data.csproj", "src/Game.Data/"]
RUN dotnet restore "./src/Game.WebAPI/Game.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Game.WebAPI"
RUN dotnet build "./Game.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Game.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.WebAPI.dll"]D:\RPSGameProject\Backend\Shared\Broker\Broker\Broker.csproj