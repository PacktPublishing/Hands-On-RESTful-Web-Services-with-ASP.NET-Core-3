FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /project
COPY ["src/VinylStore.Catalog.API/VinylStore.Catalog.API.csproj", "src/VinylStore.Catalog.API/"]
COPY . .
WORKDIR "/project/src/VinylStore.Catalog.API"
RUN dotnet build "VinylStore.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VinylStore.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VinylStore.Catalog.API.dll"]