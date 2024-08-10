FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6010
EXPOSE 6011

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Taxi.gRPC/Taxi.gRPC.csproj", "Taxi.gRPC/"]
COPY ["Taxi.Core/Taxi.Core.csproj", "Taxi.Core/"]
COPY ["Taxi.Domain/Taxi.Domain.csproj", "Taxi.Domain/"]
COPY ["Taxi.Repository/Taxi.Repository.csproj", "Taxi.Repository/"]
RUN dotnet restore "./Taxi.gRPC/Taxi.gRPC.csproj"
COPY . .
WORKDIR "/src/Taxi.gRPC"
RUN dotnet build "./Taxi.gRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Taxi.gRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taxi.gRPC.dll"]