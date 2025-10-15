# 1. Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

## Copy and restor project dependencies
COPY *.csproj ./
RUN dotnet restore

## Copy source code
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime env
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

COPY YLDatabase.db .

## Expose port + set env variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5258
EXPOSE 5258
ENTRYPOINT ["dotnet", "DigitalLibrary.API.dll"]
