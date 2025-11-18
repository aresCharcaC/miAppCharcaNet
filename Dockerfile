FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solo los .csproj primero
COPY ["MiAppCharca.Persistense/MiAppCharca.Persistense.csproj", "MiAppCharca.Persistense/"]
COPY ["MiAppCharca.Application/MiAppCharca.Application.csproj", "MiAppCharca.Application/"]
COPY ["MiAppCharca.Domain/MiAppCharca.Domain.csproj", "MiAppCharca.Domain/"]
COPY ["MiAppCharca.Infrastructure/MiAppCharca.Infrastructure.csproj", "MiAppCharca.Infrastructure/"]

# Restore
RUN dotnet restore "MiAppCharca.Persistense/MiAppCharca.Persistense.csproj"

# Copiar todo el código
COPY . .

# Build
WORKDIR "/src/MiAppCharca.Persistense"
RUN dotnet build "MiAppCharca.Persistense.csproj" -c Release -o /app/build

# Publish (con configuración para evitar conflictos)
FROM build AS publish
RUN dotnet publish "MiAppCharca.Persistense.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:EnableDefaultContentItems=false

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiAppCharca.Persistense.dll"]