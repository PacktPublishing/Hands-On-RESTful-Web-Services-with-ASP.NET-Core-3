FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview8 AS builder
WORKDIR /app
COPY . . 
RUN dotnet restore ./VinylStore.Catalog.API.sln
COPY . .

FROM builder AS publish
RUN dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VinylStore.Catalog.API.dll"]