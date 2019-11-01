FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /project
COPY ["src/Catalog.API/Catalog.API.csproj", "src/Catalog.API/"]
COPY . .
WORKDIR "/project/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]