#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /Shared
COPY ["Backend/Shared/Broker/Broker", "Broker/Broker/"]
WORKDIR /src
COPY ["Backend/Microservices/ChatService/src/Chat.WebAPI/Chat.WebAPI.csproj", "src/Chat.WebAPI/"]
COPY ["Backend/Microservices/ChatService/src/Chat.Application/Chat.Application.csproj", "src/Chat.Application/"]
COPY ["Backend/Microservices/ChatService/src/Chat.Domain/Chat.Domain.csproj", "src/Chat.Domain/"]
COPY ["Backend/Microservices/ChatService/src/Chat.Data/Chat.Data.csproj", "src/Chat.Data/"]
RUN dotnet restore "./src/Chat.WebAPI/Chat.WebAPI.csproj"
COPY ["Backend/Microservices/ChatService/src", "src/"]
WORKDIR "/src/src/Chat.WebAPI"
RUN dotnet build "./Chat.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Chat.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.WebAPI.dll"]