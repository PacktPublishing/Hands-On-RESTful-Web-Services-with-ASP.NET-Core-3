# Chapter 10 - mplementing the RESTful HTTP Layer

## Run steps

1. Pull `mcr.microsoft.com/mssql/server:2017-latest` container:

```
sudo docker pull mcr.microsoft.com/mssql/server:2017-latest
```

2. Run the container:

```
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" \
   -p 1433:1433 --name sql1 \
   -d mcr.microsoft.com/mssql/server:2017-latest
```

3. Build the project:

```
cd "Chapter 8/Catalog"
dotnet build
```

4. Test the project:

```
cd "Chapter 8/Catalog"
dotnet test
```

5. Update database with migrations:

```
cd "src/Catalog.API"
dotnet ef database update
```

6. Run the API:

```
cd "src/Catalog.API"
dotnet run
```
