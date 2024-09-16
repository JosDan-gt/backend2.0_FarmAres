# Usa la imagen de .NET SDK 8.0 para la fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos de proyecto y restaurar las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y construir la aplicación
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Usa la imagen de .NET Runtime 8.0 para la fase de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "GranjaLosAres_API.dll"]
