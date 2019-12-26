FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /project
COPY ["/src/Cart.API/Cart.API.csproj", "/src/Cart.API/"]
RUN dotnet restore "/src/Cart.API/Cart.API.csproj"
COPY . .
WORKDIR "/project/src/Cart.API"
RUN dotnet build "Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart.API.dll"]