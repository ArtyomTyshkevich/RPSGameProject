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
COPY ["Backend/Microservices/ProfileService/src/Profile.API/Profile.API.csproj", "src/Profile.API/"]
COPY ["Backend/Microservices/ProfileService/src/Profile.BLL/Profile.BLL.csproj", "src/Profile.BLL/"]
COPY ["Backend/Microservices/ProfileService/src/Profile.DAL/Profile.DAL.csproj", "src/Profile.DAL/"]
RUN dotnet restore "./src/Profile.API/Profile.API.csproj"
COPY ["Backend/Microservices/ProfileService/src", "src/"]
WORKDIR "/src/src/Profile.API"
RUN dotnet build "./Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.API.dll"]