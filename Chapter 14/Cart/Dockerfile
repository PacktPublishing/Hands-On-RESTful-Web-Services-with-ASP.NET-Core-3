FROM microsoft/dotnet:sdk AS builder
WORKDIR /app
COPY . . 
RUN dotnet restore ./VinylStore.Cart.API.sln
COPY . .

FROM builder AS publish
RUN dotnet publish -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VinylStore.Cart.API.dll"]