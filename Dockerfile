# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/src/ZooLogic.Api/ZooLogic.Api.csproj", "backend/src/ZooLogic.Api/"]
RUN dotnet restore "backend/src/ZooLogic.Api/ZooLogic.Api.csproj"

COPY ["backend/src/ZooLogic.Api/", "backend/src/ZooLogic.Api/"]
RUN dotnet publish "backend/src/ZooLogic.Api/ZooLogic.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ZooLogic.Api.dll"]
