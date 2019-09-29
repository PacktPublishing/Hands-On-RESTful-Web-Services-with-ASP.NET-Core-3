FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /project
COPY ["/src/VinylStore.Cart.API/VinylStore.Cart.API.csproj", "/src/VinylStore.Cart.API/"]
RUN dotnet restore "/src/VinylStore.Cart.API/VinylStore.Cart.API.csproj"
COPY . .
WORKDIR "/project/src/VinylStore.Cart.API"
RUN dotnet build "VinylStore.Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VinylStore.Cart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VinylStore.Cart.API.dll"]