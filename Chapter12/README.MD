# Chapter 12 - Containerization of Services

## Run steps

```
cd "Chapter 12/Catalog"
docker-compose up --build
```

_________

## Verify the implementaiton

1. Create a new genre:
```
curl -X POST \
  http://localhost:5000/api/genre \
  -H 'Content-Type: application/json' \
  -d '{
    "genreDescription": "My Genre"
}'
```

2. Create a new artist:
```
curl -X POST \
  http://localhost:5000/api/artist \
  -H 'Content-Type: application/json' \
  -d ' {
            "artistName": "My Artist"
         }'
```
3. Create a new item:
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