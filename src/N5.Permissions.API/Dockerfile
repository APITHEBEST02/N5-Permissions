# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/N5.Permissions.API/N5.Permissions.API.csproj", "N5.Permissions.API/"]
COPY ["src/N5.Permissions.Application/N5.Permissions.Application.csproj", "N5.Permissions.Application/"]
COPY ["src/N5.Permissions.Domain/N5.Permissions.Domain.csproj", "N5.Permissions.Domain/"]
COPY ["src/N5.Permissions.Infrastructure/N5.Permissions.Infrastructure.csproj", "N5.Permissions.Infrastructure/"]
COPY ["src/N5.Permissions.Shared/N5.Permissions.Shared.csproj", "N5.Permissions.Shared/"]

RUN dotnet restore "N5.Permissions.API/N5.Permissions.API.csproj"

# Copy all source code
COPY src/ .

# Build the application
WORKDIR "/src/N5.Permissions.API"
RUN dotnet build "N5.Permissions.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "N5.Permissions.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "N5.Permissions.API.dll"]
