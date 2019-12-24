FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /project
COPY ["src/Catalog.API/Catalog.API.csproj", "src/Catalog.API/"]
COPY . .
WORKDIR "/project/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_Kestrel__Certificates__Default__Password P@ssw0rd

RUN openssl genrsa -des3 -passout pass:${ASPNETCORE_Kestrel__Certificates__Default__Password} -out server.key 2048
RUN openssl rsa -passin pass:${ASPNETCORE_Kestrel__Certificates__Default__Password} -in server.key -out server.key
RUN openssl req -sha256 -new -key server.key -out server.csr -subj '/CN=localhost'
RUN openssl x509 -req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt
RUN openssl pkcs12 -export -out certificate.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:${ASPNETCORE_Kestrel__Certificates__Default__Password}

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]