# Usa la imagen de .NET SDK 8.0 para la fase de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo del proyecto específico y restaurar las dependencias
COPY ["GranjaLosAres_API.csproj", "./"]
RUN dotnet restore

# Copiar el resto del código fuente y construir la aplicación
COPY . .
RUN dotnet publish -c Release -o /app/out

# Usa la imagen de .NET Runtime 8.0 para la fase de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Definir el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "GranjaLosAres_API.dll"]

# Definir las variables de entorno
ENV Jwt__Key "UnaNuevaClaveSuperSeguraYMuyLargaDeAlMenos32Caracteres"
