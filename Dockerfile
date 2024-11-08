FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto y restaurar dependencias
COPY [".", "./"]
RUN dotnet restore "./CrackerBarrelPegGame/CrackerBarrelPegGame.csproj"

# Copiar el resto del código y compilar la aplicación
COPY . .
RUN dotnet publish "./CrackerBarrelPegGame/CrackerBarrelPegGame.csproj" -c Release -o /app/publish

# Usar una imagen más ligera para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CrackerBarrelPegGame.dll"]