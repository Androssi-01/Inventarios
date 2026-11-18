# 1. Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos el csproj y restauramos dependencias
COPY ["Portafolio.API.csproj", "./"]
RUN dotnet restore

# Copiamos todo el código y compilamos
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 2. Etapa de ejecución (Runtime - imagen mucho más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponemos el puerto (8080 es el estándar en .NET 8+)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Portafolio.API.dll"]