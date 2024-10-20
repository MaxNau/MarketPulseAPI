# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 32772:80
EXPOSE 32773:443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MarketPulseAPI/MarketPulseAPI.csproj", "src/MarketPulseAPI/"]
COPY ["src/MarketPriceAPI.Configurations/MarketPriceAPI.Configurations.csproj", "src/MarketPriceAPI.Configurations/"]
COPY ["src/MarketPriceAPI.Data/MarketPriceAPI.Data.csproj", "src/MarketPriceAPI.Data/"]
COPY ["src/MarketPriceAPI.Data.Entities/MarketPriceAPI.Data.Entities.csproj", "src/MarketPriceAPI.Data.Entities/"]
COPY ["src/MarketPulseAPI.Fintacharts.Models/MarketPulseAPI.Fintacharts.Models.csproj", "src/MarketPulseAPI.Fintacharts.Models/"]
COPY ["src/MarketPulseAPI.Util/MarketPulseAPI.Util.csproj", "src/MarketPulseAPI.Util/"]
COPY ["src/MarketPulseAPI.Interfaces/MarketPulseAPI.Interfaces.csproj", "src/MarketPulseAPI.Interfaces/"]
COPY ["src/MarketPulseAPI.Automation/MarketPulseAPI.Automation.csproj", "src/MarketPulseAPI.Automation/"]
COPY ["src/MarketPulseAPI.Services/MarketPulseAPI.Services.csproj", "src/MarketPulseAPI.Services/"]

COPY src/MarketPulseAPI/wait-for-it.sh ./

RUN dotnet restore "./src/MarketPulseAPI/MarketPulseAPI.csproj"
COPY . .
WORKDIR "/src/src/MarketPulseAPI"

RUN dotnet build "./MarketPulseAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MarketPulseAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["sh", "-c", "echo 'Waiting for the database...'; ./wait-for-it.sh marketpulseapi-db:1433 --timeout=30 --strict -- echo 'Database is up!'; dotnet MarketPulseAPI.dll"]