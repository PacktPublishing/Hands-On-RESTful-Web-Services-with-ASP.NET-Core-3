# Chapter 8 - Building the Data Access Layer

## Run steps

1. Install `ef-core` global tools:

```
dotnet tool install --global dotnet-ef
```

2. Pull `mssql/server:2017-latest` container:

```
sudo docker pull mcr.microsoft.com/mssql/server:2017-latest
```

3. Run the container:

```
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" \
   -p 1433:1433 --name sql1 \
   -d mcr.microsoft.com/mssql/server:2017-latest
```

4. Build the project:

```
cd "Chapter 8/Catalog"
dotnet build
```

5. Test the project:

```
cd "Chapter 8/Catalog"
dotnet test
```

6. Update database with migrations:

```
cd "src/Catalog.API"
dotnet ef database update
```