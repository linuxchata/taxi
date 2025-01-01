FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 6001
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Taxi.WebApi/Taxi.WebApi.csproj", "Taxi.WebApi/"]
COPY ["Taxi.Core/Taxi.Core.csproj", "Taxi.Core/"]
COPY ["Taxi.Domain/Taxi.Domain.csproj", "Taxi.Domain/"]
COPY ["Taxi.Repository/Taxi.Repository.csproj", "Taxi.Repository/"]
RUN dotnet restore "./Taxi.WebApi/Taxi.WebApi.csproj"
COPY . .
WORKDIR "/src/Taxi.WebApi"
RUN dotnet build "./Taxi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Taxi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taxi.WebApi.dll"]