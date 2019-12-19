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
cd "Chapter 10/Catalog"
dotnet build
```

4. Test the project:

```
cd "Chapter 10/Catalog"
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
7. Verify the implementation:

   - Create a new genre:
```
curl -X POST \
  http://localhost:5000/api/genre \
  -H 'Content-Type: application/json' \
  -d '{
    "genreDescription": "My Genre"
}'
```
   - Create a new artist:
```
curl -X POST \
  http://localhost:5000/api/genre \
  -H 'Content-Type: application/json' \
  -d ' {
            "artistName": "My Artist"
         }'
```
   - Create a new item:
```
curl -X POST \
  http://localhost:5000/api/items \
  -H 'Content-Type: application/json' \
  -d '{
            "name": "Test",
            "description": "Description",
            "labelName": "Label",
            "price": {
               "currency": "EUR",
               "amount": 34.3
            },
            "pictureUri": "",
            "releaseDate": "2019-11-21T16:18:42+00:00",
            "format": "",
            "availableStock": 4,
            "genreId":"<genre_id>",
            "artistId":"<artist_id>"
         }'
```

