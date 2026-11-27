# syntax=docker/dockerfile:1

# 1) Compilar el frontend Angular (requiere Node 24 o superior).
FROM node:24-alpine AS frontend
WORKDIR /src/frontend

COPY frontend/package.json frontend/package-lock.json ./
# npm ci exige un lock con las dependencias del sistema destino. El lock se genera
# en Windows y no incluye las de Linux, asi que aqui se resuelven con npm install.
RUN npm install --no-audit --no-fund

COPY frontend/ ./
RUN npm run build

# 2) Publicar la API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/src/ZooLogic.Api/ZooLogic.Api.csproj", "backend/src/ZooLogic.Api/"]
RUN dotnet restore "backend/src/ZooLogic.Api/ZooLogic.Api.csproj"

COPY ["backend/src/ZooLogic.Api/", "backend/src/ZooLogic.Api/"]
RUN dotnet publish "backend/src/ZooLogic.Api/ZooLogic.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# 3) La API sirve el frontend desde wwwroot: una sola URL y un solo servicio.
COPY --from=frontend /src/frontend/dist/frontend/browser/ /app/publish/wwwroot/

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ZooLogic.Api.dll"]
